using System.Linq.Expressions;
using Iprox.Domain.Interface.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Iprox.Infrastructure.Persistence
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException("context");
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            return _context.Set<TEntity>().Where(expression);
        }

        public IQueryable<TEntity> AsQueryable()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task RemoveAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            await Task.CompletedTask;
        }

        public async Task RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
            await Task.CompletedTask;
        }

        public async Task EntityStateModifiedAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await Task.CompletedTask;
        }

        public IQueryable<TEntity> Include(Expression<Func<TEntity, object>> expression)
        {
            return _context.Set<TEntity>().Include(expression);
        }

        public IQueryable<TEntity>? IncludeMultipleAsync(IEnumerable<Expression<Func<TEntity, object>>> expressions)
        {
            Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<TEntity, object> query = null;
            int count = 0;
            foreach (var include in expressions)
            {
                if (count == 0)
                {
                    query = _context.Set<TEntity>().Include(include);
                    count++;
                }
                else
                {
                    query = query.Include(include);
                }
            }
            return query.AsQueryable();
        }
    }
}
