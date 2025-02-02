namespace Iprox.Domain.Interface.IRepositories;

public interface IUnitOfWork
{
    ITvShowRepository TvShowRepository { get; }
    IGenreRepository GenreRepository { get; }

    Task<int> CompleteAsync();
    Task DisposeAsync();
}
