using Microsoft.EntityFrameworkCore.Diagnostics;
using Ordering.Domain.Abstractions;
using Ordering.Infrastructure.Data.Extensions;

namespace Ordering.Infrastructure.Data.Interceptors;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = new())
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateEntities(DbContext? eventDataContext)
    {
        if (eventDataContext is null) return;

        foreach (var entry in eventDataContext.ChangeTracker.Entries<IEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = "I am";
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }

            if (entry.State is EntityState.Added or EntityState.Modified
                || entry.HasChangedOwnedEntities())
            {
                entry.Entity.LastModifiedBy = "I am";
                entry.Entity.LastModified = DateTime.UtcNow;
            }
        }
    }
}