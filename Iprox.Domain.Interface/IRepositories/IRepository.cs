using System.Linq.Expressions;

namespace Iprox.Domain.Interface.IRepositories;

public interface IRepository<TEntity> where TEntity : class
{
    // Async methods for CRUD operations
    Task<TEntity> AddAsync(TEntity entity);
    Task AddRangeAsync(IEnumerable<TEntity> entities);
    Task<TEntity?> GetByIdAsync(int id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task RemoveAsync(TEntity entity);
    Task RemoveRangeAsync(IEnumerable<TEntity> entities);
    Task EntityStateModifiedAsync(TEntity entity);

    // Synchronous methods for querying
    IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression);
    IQueryable<TEntity> AsQueryable();
    IQueryable<TEntity> Include(Expression<Func<TEntity, object>> expression);
    IQueryable<TEntity>? IncludeMultipleAsync(IEnumerable<Expression<Func<TEntity, object>>> expressions);
}
