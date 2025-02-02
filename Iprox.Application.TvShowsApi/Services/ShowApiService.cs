using AutoMapper;
using Iprox.Application.Common.Dtos;
using Iprox.Application.TvShowsApi.Interfaces;
using Iprox.Domain.Entities;
using Iprox.Domain.Interface;
using Iprox.Domain.Interface.IRepositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Iprox.Application.TvShowsApi.Services;

public class ShowApiService : IShowApiService
{
    private readonly ILogger<ShowApiService> _logger;
    private readonly IShowCoreService _showCoreService;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;
    private readonly IUnitOfWork _unitOfWork;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(10);
    private const string CacheKeyAllShows = "AllTvShows";

    public ShowApiService(ILogger<ShowApiService> logger, IShowCoreService showCoreService, IMapper mapper, IMemoryCache cache, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _showCoreService = showCoreService;
        _mapper = mapper;
        _cache = cache;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<TvShowDto>> GetAllAsync()
    {
        try
        {
            if (!_cache.TryGetValue(CacheKeyAllShows, out IEnumerable<TvShowDto>? tvShowDtoResponse))
            {
                IEnumerable<TvShow> tvShows = await _showCoreService.GetAllAsync();
                tvShowDtoResponse = _mapper.Map<IEnumerable<TvShowDto>>(tvShows);

                _cache.Set(CacheKeyAllShows, tvShowDtoResponse, _cacheDuration);
            }
            return tvShowDtoResponse!;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching TV shows.");
            return Enumerable.Empty<TvShowDto>();
        }
    }

    public async Task<PagedResult<TvShowDto>> GetGridDataAsync(int? page, int? pageSize, string? search, string? sortBy, bool? descending)
    {
        try
        {
            //When parameters not provided but still wants to search or sort need to set default values
            page ??= 1;
            pageSize ??= 10;
            descending ??= false;

            IQueryable<TvShow> tvShowsQuery = _unitOfWork.TvShowRepository.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                tvShowsQuery = tvShowsQuery.Where(ts => ts.Name.Contains(search));
            }

            if (!string.IsNullOrEmpty(sortBy) || descending.HasValue == true)
            {
                switch (sortBy?.ToLower())
                {
                    case "name":
                        tvShowsQuery = descending.Value ? tvShowsQuery.OrderByDescending(ts => ts.Name) : tvShowsQuery.OrderBy(ts => ts.Name);
                        break;
                    case "language":
                        tvShowsQuery = descending.Value ? tvShowsQuery.OrderByDescending(ts => ts.Language) : tvShowsQuery.OrderBy(ts => ts.Language);
                        break;
                    case "premiered":
                        tvShowsQuery = descending.Value ? tvShowsQuery.OrderByDescending(ts => ts.Premiered) : tvShowsQuery.OrderBy(ts => ts.Premiered);
                        break;
                    default:
                        tvShowsQuery = descending.Value ? tvShowsQuery.OrderByDescending(ts => ts.Id) : tvShowsQuery.OrderBy(ts => ts.Id);
                        break;
                }
            }

            int totalCount = tvShowsQuery.Count();

            List<TvShow> tvShows = tvShowsQuery.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();

            List<TvShowDto> tvShowDtoResponse = _mapper.Map<List<TvShowDto>>(tvShows);

            return new PagedResult<TvShowDto>(totalCount, tvShowDtoResponse, page.Value, pageSize.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching TV shows.");
            return new PagedResult<TvShowDto>(0, Enumerable.Empty<TvShowDto>(), page.Value, pageSize.Value);
        }
    }

    public async Task<TvShowDto?> GetByIdAsync(int id)
    {
        try
        {
            string cacheKey = $"TvShow_{id}";
            if (!_cache.TryGetValue(cacheKey, out TvShowDto? tvShowDto))
            {
                TvShow? tvShow = await _showCoreService.GetByIdAsync(id);
                if (tvShow == null) return null;

                tvShowDto = _mapper.Map<TvShowDto>(tvShow);
                _cache.Set(cacheKey, tvShowDto, _cacheDuration);
            }
            return tvShowDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching the TV show.");
            return null;
        }
    }

    public async Task<TvShowDto?> CreateAsync(CreateTvShowDto tvShowDto)
    {
        try
        {
            TvShow tvShow = _mapper.Map<TvShow>(tvShowDto);
            TvShow? createdTvShow = await _showCoreService.CreateAsync(tvShow);

            if (createdTvShow != null)
            {
                // Invalidate cache to ensure new data is included
                _cache.Remove(CacheKeyAllShows);
            }

            return _mapper.Map<TvShowDto>(createdTvShow);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the TV show.");
            return null;
        }
    }

    public async Task<bool> UpdateAsync(int id, TvShowDto tvShowDto)
    {
        try
        {
            TvShow tvShow = _mapper.Map<TvShow>(tvShowDto);
            bool isUpdated = await _showCoreService.UpdateAsync(id, tvShow);

            if (isUpdated)
            {
                // Invalidate cache for this specific show and all shows
                _cache.Remove($"TvShow_{id}");
                _cache.Remove(CacheKeyAllShows);
            }

            return isUpdated;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the TV show.");
            return false;
        }
    }

    public async Task<bool> PatchAsync(int id, PatchTvShowDto patchDto)
    {
        try
        {
            TvShow? existingTvShow = await _showCoreService.GetByIdAsync(id);
            if (existingTvShow == null) return false;

            if (patchDto.Name != null) existingTvShow.Name = patchDto.Name;
            if (patchDto.Summary != null) existingTvShow.Summary = patchDto.Summary;

            bool isUpdated = await _showCoreService.UpdateAsync(id, existingTvShow);

            if (isUpdated)
            {
                _cache.Remove($"TvShow_{id}");
                _cache.Remove(CacheKeyAllShows);
            }

            return isUpdated;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while applying the patch to the TV show.");
            return false;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            bool isDeleted = await _showCoreService.DeleteAsync(id);
            if (isDeleted)
            {
                _cache.Remove($"TvShow_{id}");
                _cache.Remove(CacheKeyAllShows);
            }
            return isDeleted;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the TV show.");
            return false;
        }
    }
}
