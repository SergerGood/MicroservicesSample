using Microsoft.EntityFrameworkCore.ChangeTracking;
using Ordering.Domain.Abstractions;

namespace Ordering.Infrastructure.Data.Extensions;

public static class EntityEntryExtensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry<IEntity> entry) =>
        entry.References.Any(x =>
            x.TargetEntry is not null
            && x.TargetEntry.Metadata.IsOwned()
            && x.TargetEntry.State is EntityState.Added or EntityState.Modified);
}