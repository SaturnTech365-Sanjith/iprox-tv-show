using System.Linq.Expressions;
using Iprox.Domain.Entities;
using Iprox.Domain.Interface;
using Iprox.Domain.Interface.IRepositories;
using Microsoft.Extensions.Logging;

namespace Iprox.Domain.Core
{
    public class ShowCoreService : IShowCoreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ShowCoreService> _logger;

        public ShowCoreService(IUnitOfWork unitOfWork, ILogger<ShowCoreService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<TvShow>> GetAllAsync()
        {
            try
            {
                var filterExpression = (Expression<Func<TvShow, object>>)(t => t.Genres);
                IEnumerable<TvShow> tvShows = _unitOfWork.TvShowRepository.Include(filterExpression).ToList();
                return tvShows;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all TV shows.");
                throw;
            }
        }

        public async Task<TvShow?> GetByIdAsync(int id)
        {
            try
            {
                TvShow? tvShow = await _unitOfWork.TvShowRepository.GetByIdAsync(id);
                return tvShow;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the TV show.");
                throw;
            }
        }

        public async Task<TvShow> CreateAsync(TvShow tvShow)
        {
            try
            {
                List<Genre> refGenres = new List<Genre>();

                if (tvShow?.Genres != null)
                {
                    foreach (var genre in tvShow.Genres)
                    {
                        var existingGenre = await _unitOfWork.GenreRepository.GetByIdAsync(genre.Id);

                        if (existingGenre != null)
                        {
                            refGenres.Add(existingGenre);
                        }
                    }
                    tvShow.Genres = refGenres.DistinctBy(g => g.Id).ToList();
                }
                else
                {
                    tvShow.Genres = null;
                }

                await _unitOfWork.TvShowRepository.AddAsync(tvShow);
                int saved = await _unitOfWork.CompleteAsync();

                if (saved > 0)
                {
                    return tvShow;
                }

                throw new Exception("Failed to create the TV show. No changes were made to the database.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the TV show.");
                throw;
            }
        }

        public async Task<List<TvShow>> CreateAsync(List<TvShow> tvShows)
        {
            try
            {
                IEnumerable<Genre> getAllGenre = await _unitOfWork.GenreRepository.GetAllAsync();
                foreach (var tvShow in tvShows)
                {
                    List<Genre> refGenres = new List<Genre>();

                    if (tvShow?.Genres != null)
                    {
                        foreach (var genre in tvShow.Genres)
                        {
                            var existingGenre = getAllGenre.Where(x => x.Id == genre.Id).FirstOrDefault();

                            if (existingGenre != null)
                            {
                                refGenres.Add(existingGenre);
                            }
                        }
                        tvShow.Genres = refGenres.DistinctBy(g => g.Id).ToList();
                    }
                    else
                    {
                        tvShow.Genres = null;
                    }
                }

                await _unitOfWork.TvShowRepository.AddRangeAsync(tvShows);
                int saved = await _unitOfWork.CompleteAsync();

                if (saved > 0)
                {
                    return tvShows;
                }

                throw new Exception("Failed to create TV shows. No changes were made to the database.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating TV shows.");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(int id, TvShow tvShow)
        {
            try
            {
                var filterExpression = (Expression<Func<TvShow, object>>)(t => t.Genres);
                TvShow? existingTvShow = _unitOfWork.TvShowRepository.Include(filterExpression).FirstOrDefault();
                if (existingTvShow == null)
                {
                    return false;
                }

                existingTvShow.Name = tvShow.Name;
                existingTvShow.Language = tvShow.Language;
                existingTvShow.Premiered = tvShow.Premiered;
                existingTvShow.Summary = tvShow.Summary;
                existingTvShow.TvMazeId = tvShow.TvMazeId;
                List<Genre> refGenres = new List<Genre>();

                if (tvShow?.Genres != null)
                {
                    foreach (var genre in tvShow.Genres)
                    {
                        var existingGenre = await _unitOfWork.GenreRepository.GetByIdAsync(genre.Id);

                        if (existingGenre != null)
                        {
                            refGenres.Add(existingGenre);
                        }
                    }
                    existingTvShow.Genres = refGenres.DistinctBy(g => g.Id).ToList();
                }
                else
                {
                    existingTvShow.Genres?.Clear();
                }

                await _unitOfWork.TvShowRepository.EntityStateModifiedAsync(existingTvShow);

                int saved = await _unitOfWork.CompleteAsync();
                return saved > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the TV show.");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                TvShow? tvShow = await _unitOfWork.TvShowRepository.GetByIdAsync(id);
                if (tvShow == null)
                {
                    return false;
                }

                await _unitOfWork.TvShowRepository.RemoveAsync(tvShow);
                int saved = await _unitOfWork.CompleteAsync();
                return saved > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the TV show.");
                throw;
            }
        }
    }
}
