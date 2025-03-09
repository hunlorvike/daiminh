using Microsoft.EntityFrameworkCore.Storage;
using shared.Models;

namespace shared.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;
    Task<int> SaveChangesAsync();
    Task<IDbContextTransaction?> BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}