using System.Linq.Expressions;
using core.Entities;

namespace core.Interfaces;

public interface IRepository<TEntity, in TKey> where TEntity : BaseEntity<TKey>
{
    Task<TEntity?> FindByIdAsync(TKey id);
    Task<IEnumerable<TEntity>> FindAllAsync();
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

    Task AddAsync(TEntity entity);
    Task AddRangeAsync(IEnumerable<TEntity> entities);

    Task UpdateAsync(TEntity entity);
    Task UpdateRangeAsync(IEnumerable<TEntity> entities);

    Task DeleteAsync(TEntity entity);
    Task DeleteRangeAsync(IEnumerable<TEntity> entities);

    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null);

    IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
}