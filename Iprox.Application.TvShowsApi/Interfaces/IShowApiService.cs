
using Iprox.Application.Common.Dtos;

namespace Iprox.Application.TvShowsApi.Interfaces;

public interface IShowApiService
{
    Task<IEnumerable<TvShowDto>> GetAllAsync();
    Task<PagedResult<TvShowDto>> GetGridDataAsync(int? page, int? pageSize, string? search, string? sortBy, bool? descending);
    Task<TvShowDto?> GetByIdAsync(int id);
    Task<TvShowDto?> CreateAsync(CreateTvShowDto tvShowDto);
    Task<bool> UpdateAsync(int id, TvShowDto tvShowDto);
    Task<bool> PatchAsync(int id, PatchTvShowDto patchDto);
    Task<bool> DeleteAsync(int id);
}