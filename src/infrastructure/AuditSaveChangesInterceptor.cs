using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using shared.Models;

namespace infrastructure;

/// <summary>
/// Interceptor for Entity Framework Core SaveChanges operations that automatically updates audit fields (CreatedAt, UpdatedAt, DeletedAt)
/// on entities derived from <see cref="BaseEntity"/>. This handles soft deletes by marking entities as deleted instead of physically removing them.
/// </summary>
public class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    /// <summary>
    /// Updates the audit fields (CreatedAt, UpdatedAt, DeletedAt) of entities based on their state.
    /// </summary>
    /// <param name="context">The DbContext instance.</param>
    private static void UpdateAuditFields(DbContext? context)
    {
        if (context == null)
            return;

        // Iterate through all tracked entities that inherit from BaseEntity
        foreach (var entry in context.ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    // For newly added entities, set CreatedAt and UpdatedAt to the current UTC time.
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    // For modified entities, update UpdatedAt to the current UTC time.
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Deleted:
                    // For "deleted" entities, implement soft delete:
                    // 1. Change the state to Modified (so EF Core updates instead of deletes).
                    // 2. Set DeletedAt to the current UTC time.
                    entry.State = EntityState.Modified;
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                    break;
            }
        }
    }

    /// <summary>
    /// Overrides the SavingChanges method to update audit fields before the changes are saved to the database. (Synchronous version)
    /// </summary>
    /// <param name="eventData">Data related to the save changes event.</param>
    /// <param name="result">The current interception result.</param>
    /// <returns>The interception result.</returns>
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        UpdateAuditFields(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    /// <summary>
    /// Overrides the SavingChangesAsync method to update audit fields before the changes are saved to the database. (Asynchronous version)
    /// </summary>
    /// <param name="eventData">Data related to the save changes event.</param>
    /// <param name="result">The current interception result.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the interception result.</returns>
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateAuditFields(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}