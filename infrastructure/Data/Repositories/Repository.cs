using core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using core.Entities.Shared;

namespace infrastructure.Data.Repositories;

public class Repository<TEntity, TKey>(ApplicationDbContext context) : IRepository<TEntity, TKey>
    where TEntity : BaseEntity<TKey>
{
    protected readonly ApplicationDbContext _context = context;
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    public async Task<TEntity?> FindByIdAsync(TKey id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> FindAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public Task UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task UpdateRangeAsync(IEnumerable<TEntity> entities)
    {
        _dbSet.UpdateRange(entities);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public Task DeleteRangeAsync(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        return predicate == null ? await _dbSet.CountAsync() : await _dbSet.CountAsync(predicate);
    }

    public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbSet.Where(predicate);
    }

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        return predicate == null
            ? await _dbSet.FirstOrDefaultAsync()
            : await _dbSet.FirstOrDefaultAsync(predicate);
    }

    public async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        return predicate == null
            ? await _dbSet.FirstAsync()
            : await _dbSet.FirstAsync(predicate);
    }

    public async Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.SingleOrDefaultAsync(predicate);
    }

    public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.SingleAsync(predicate);
    }

    public async Task<TEntity?> LastOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        var query = predicate == null ? _dbSet : _dbSet.Where(predicate);
        return await query.LastOrDefaultAsync();
    }

    public async Task<TEntity> LastAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        var query = predicate == null ? _dbSet : _dbSet.Where(predicate);
        return await query.LastAsync();
    }

    public async Task<List<TEntity>> ToListAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public IQueryable<TEntity> OrderBy<TKey1>(Expression<Func<TEntity, TKey1>> keySelector)
    {
        return _dbSet.OrderBy(keySelector);
    }

    public IQueryable<TEntity> OrderByDescending<TKey1>(Expression<Func<TEntity, TKey1>> keySelector)
    {
        return _dbSet.OrderByDescending(keySelector);
    }

    public async Task<TEntity?> MaxAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        var query = predicate == null ? _dbSet : _dbSet.Where(predicate);
        return await query.MaxAsync();
    }

    public async Task<TEntity?> MinAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        var query = predicate == null ? _dbSet : _dbSet.Where(predicate);
        return await query.MinAsync();
    }

    public async Task<decimal> AverageAsync(Expression<Func<TEntity, decimal>> selector)
    {
        return await _dbSet.AverageAsync(selector);
    }

    public async Task<decimal> SumAsync(Expression<Func<TEntity, decimal>> selector)
    {
        return await _dbSet.SumAsync(selector);
    }

    public IQueryable<TResult> Select<TResult>(Expression<Func<TEntity, TResult>> selector)
    {
        return _dbSet.Select(selector);
    }

    public async Task<bool> AllAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.AllAsync(predicate);
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        return predicate == null
            ? await _dbSet.AnyAsync()
            : await _dbSet.AnyAsync(predicate);
    }

    public IQueryable<TEntity> Skip(int count)
    {
        return _dbSet.Skip(count);
    }

    public IQueryable<TEntity> Take(int count)
    {
        return _dbSet.Take(count);
    }

    public IQueryable<TEntity> Include<TProperty>(Expression<Func<TEntity, TProperty>> navigationPropertyPath)
    {
        return _dbSet.Include(navigationPropertyPath);
    }

    public IQueryable<TEntity> AsNoTracking()
    {
        return _dbSet.AsNoTracking();
    }

    public async Task<Dictionary<TKey1, TEntity>> ToDictionaryAsync<TKey1>(
        Expression<Func<TEntity, TKey1>> keySelector) where TKey1 : notnull
    {
        return await _dbSet.ToDictionaryAsync(keySelector.Compile());
    }

    public async Task<IEnumerable<TResult>> GroupByAsync<TKey1, TResult>(
        Expression<Func<TEntity, TKey1>> keySelector,
        Expression<Func<IGrouping<TKey1, TEntity>, TResult>> resultSelector)
    {
        return await _dbSet
            .GroupBy(keySelector)
            .Select(resultSelector)
            .ToListAsync();
    }
}