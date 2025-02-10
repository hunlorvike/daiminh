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

    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null);
    Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>>? predicate = null);

    Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);

    Task<TEntity?> LastOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null);
    Task<TEntity> LastAsync(Expression<Func<TEntity, bool>>? predicate = null);

    Task<List<TEntity>> ToListAsync();

    IQueryable<TEntity> OrderBy<TKey>(Expression<Func<TEntity, TKey>> keySelector);
    IQueryable<TEntity> OrderByDescending<TKey>(Expression<Func<TEntity, TKey>> keySelector);

    Task<TEntity?> MaxAsync(Expression<Func<TEntity, bool>>? predicate = null);
    Task<TEntity?> MinAsync(Expression<Func<TEntity, bool>>? predicate = null);

    Task<decimal> AverageAsync(Expression<Func<TEntity, decimal>> selector);
    Task<decimal> SumAsync(Expression<Func<TEntity, decimal>> selector);

    IQueryable<TResult> Select<TResult>(Expression<Func<TEntity, TResult>> selector);

    Task<bool> AllAsync(Expression<Func<TEntity, bool>> predicate);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null);

    IQueryable<TEntity> Skip(int count);
    IQueryable<TEntity> Take(int count);

    IQueryable<TEntity> Include<TProperty>(Expression<Func<TEntity, TProperty>> navigationPropertyPath);

    IQueryable<TEntity> AsNoTracking();

    Task<Dictionary<TKey, TEntity>> ToDictionaryAsync<TKey>(
        Expression<Func<TEntity, TKey>> keySelector) where TKey : notnull;

    Task<IEnumerable<TResult>> GroupByAsync<TKey, TResult>(
        Expression<Func<TEntity, TKey>> keySelector,
        Expression<Func<IGrouping<TKey, TEntity>, TResult>> resultSelector);
}