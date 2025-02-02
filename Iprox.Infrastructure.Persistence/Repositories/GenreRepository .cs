using Iprox.Domain.Entities;
using Iprox.Domain.Interface.IRepositories;

namespace Iprox.Infrastructure.Persistence.Repositories;

public class TvShowRepository : Repository<TvShow>, ITvShowRepository
{
    public TvShowRepository(ApplicationDbContext context) : base(context)
    {
    }
}
