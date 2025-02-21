using core.Entities;
using core.Entities.Shared;
using Microsoft.EntityFrameworkCore.Storage;

namespace core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;
    Task<int> SaveChangesAsync();
    Task<IDbContextTransaction?> BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}