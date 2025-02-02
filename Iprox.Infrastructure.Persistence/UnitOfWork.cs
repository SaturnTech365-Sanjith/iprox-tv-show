using Iprox.Domain.Interface.IRepositories;
using Iprox.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Iprox.Infrastructure.Persistence;

public class UnitOfWork :  IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        TvShowRepository = new TvShowRepository(_context);
        GenreRepository = new GenreRepository(_context);
    }

    public ITvShowRepository TvShowRepository { get; private set; }
    public IGenreRepository GenreRepository { get; private set; }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}
