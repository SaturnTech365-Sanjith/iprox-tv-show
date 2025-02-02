using Iprox.Domain.Entities;

namespace Iprox.Domain.Interface;

public interface IShowCoreService
{
    Task<IEnumerable<TvShow>> GetAllAsync();
    Task<TvShow?> GetByIdAsync(int id);
    Task<TvShow> CreateAsync(TvShow tvShow);
    Task<List<TvShow>> CreateAsync(List<TvShow> tvShows);
    Task<bool> UpdateAsync(int id, TvShow tvShow);
    Task<bool> DeleteAsync(int id);
}
