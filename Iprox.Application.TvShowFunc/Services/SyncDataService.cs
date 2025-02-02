using Iprox.Application.Common.Dtos;
using Iprox.Application.Common.Interface;
using Iprox.Application.TvShowFunc.Interfaces;
using Iprox.Application.TvShowsApi.Services;
using Iprox.Domain.Entities;
using Iprox.Domain.Helpers;
using Iprox.Domain.Interface;
using Iprox.Domain.Interface.IRepositories;
using Microsoft.Extensions.Logging;

namespace Iprox.Application.TvShowFunc.Services;

public class SyncDataService : ISyncDataService
{
    private readonly ILogger<ShowApiService> _logger;
    private readonly IShowCoreService _showCoreService;
    private readonly IExternalServiceClient _externalServiceClient;
    private readonly IUnitOfWork _unitOfWork;

    private const int MaxRecordsPerExecution = 250;
    private const int PageSize = 249;

    public SyncDataService(ILogger<ShowApiService> logger, IShowCoreService showCoreService, IExternalServiceClient externalServiceClient, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _showCoreService = showCoreService;
        _externalServiceClient = externalServiceClient;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> SyncDataAsync()
    {
        try
        {
            _logger.LogInformation("Starting TV show sync process...");

            int? maxTvMazeId = _unitOfWork.TvShowRepository.AsQueryable().Max(x => x.TvMazeId);
            int startTvMazeId = maxTvMazeId.HasValue ? maxTvMazeId.Value + 1 : 1;
            int currentPage = startTvMazeId / PageSize;
            int recordsProcessed = 0;

            while (recordsProcessed < MaxRecordsPerExecution)
            {
                // Get TV shows from the API for the current page
                List<TvMazeResponseDto> tvShowsFromApi = await _externalServiceClient.GetTvShowsByPageAsync(currentPage);

                if (tvShowsFromApi == null || !tvShowsFromApi.Any())
                {
                    break;
                }

                List<TvShow> newTvShows = tvShowsFromApi
                                              .Where(x => x.Id >= startTvMazeId)
                                              .Take(MaxRecordsPerExecution - recordsProcessed)
                                              .Select(x => new TvShow
                                              {
                                                  Name = x.Name ?? "N/A",
                                                  Language = x.Language,
                                                  Premiered = DateOnly.TryParse(x.Premiered, out var date) ? date : (DateOnly?)null,
                                                  Genres = x.Genres?
                                                           .Select(g => EnumHelper.GetGenre(g.Trim()))
                                                           .DistinctBy(g => g.Id)
                                                           .ToList(),
                                                  Summary = x.Summary,
                                                  TvMazeId = x.Id
                                              })
                                              .ToList();

                if (newTvShows.Any())
                {
                    await _showCoreService.CreateAsync(newTvShows);
                }
                break;
            }

            _logger.LogInformation("TV show sync process completed successfully.");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during the TV show sync process.");
            return false;
        }
    }
}

