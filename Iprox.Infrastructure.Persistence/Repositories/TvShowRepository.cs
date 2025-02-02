using Iprox.Domain.Entities;
using Iprox.Domain.Interface.IRepositories;

namespace Iprox.Infrastructure.Persistence.Repositories;

public class GenreRepository : Repository<Genre>, IGenreRepository
{
    public GenreRepository(ApplicationDbContext context) : base(context)
    {
    }
}
