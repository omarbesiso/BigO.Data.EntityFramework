using BigO.Core.Validation;
using Microsoft.EntityFrameworkCore;

// ReSharper disable SwitchStatementMissingSomeEnumCasesNoDefault

namespace BigO.Data.EntityFramework.Auditing;

internal static class AuditExtensions
{
    internal static void AuditEntries(this DbContext? context, object? actorId = null)
    {
        Guard.NotNull(context, nameof(context));

        var changedEntries = context.ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted).ToList();

        foreach (var entry in changedEntries)
        {
            var trackingSupport = AuditCache.GetEntityAuditSupport(entry.Metadata.ClrType);

            switch (entry.State)
            {
                case EntityState.Added:
                {
                    if (trackingSupport.SupportsCreationTimestamp)
                    {
                        entry.Property(nameof(IEntityWithCreationTimeStamp.CreatedOnUtc)).CurrentValue =
                            DateTime.UtcNow;
                    }

                    if (trackingSupport.SupportsCreationActorAudits)
                    {
                        entry.Property(nameof(IEntityWithCreationActorAudit<int>.CreatedBy)).CurrentValue = actorId;
                    }

                    if (trackingSupport.SupportsVersioning)
                    {
                        entry.Property(nameof(IEntityWithVersionNumber.VersionNumber)).CurrentValue = 1;
                    }

                    break;
                }
                case EntityState.Modified:
                {
                    if (trackingSupport.SupportsModificationTimestamp)
                    {
                        entry.Property(nameof(IEntityWithModificationTimeStamp.ModifiedOnUtc)).CurrentValue =
                            DateTime.UtcNow;
                    }

                    if (trackingSupport.SupportsModificationActorAudits)
                    {
                        entry.Property(nameof(IEntityWithModificationActorAudit<int>.ModifiedBy)).CurrentValue =
                            actorId;
                    }

                    if (trackingSupport.SupportsVersioning)
                    {
                        const string versionNumberName = nameof(IEntityWithVersionNumber.VersionNumber);
                        var versionValue = Convert.ToInt32(entry.Property(versionNumberName).OriginalValue);
                        entry.Property(versionNumberName).CurrentValue = versionValue + 1;
                    }

                    break;
                }
                case EntityState.Deleted:
                {
                    var requiresVersioning = false;

                    if (trackingSupport.SupportsSoftDelete)
                    {
                        requiresVersioning = true;
                        entry.State = EntityState.Modified;
                        entry.Property(nameof(IEntityWithSoftDelete.IsDeleted)).CurrentValue = true;
                    }

                    if (trackingSupport.SupportsSoftDeleteTimestamps)
                    {
                        requiresVersioning = true;
                        entry.Property(nameof(IEntityWithSoftDeleteTimestamp.DeletedOnUtc)).CurrentValue =
                            DateTime.UtcNow;
                    }

                    if (trackingSupport.SupportSoftDeleteActorAudits)
                    {
                        entry.Property(nameof(IEntityWithSoftDeleteActorAudit<int>.DeletedBy)).CurrentValue =
                            actorId;
                    }

                    if (requiresVersioning && trackingSupport.SupportsVersioning)
                    {
                        const string versionNumberName = nameof(IEntityWithVersionNumber.VersionNumber);
                        var versionValue = Convert.ToInt32(entry.Property(versionNumberName).OriginalValue);
                        entry.Property(versionNumberName).CurrentValue = versionValue + 1;
                    }

                    break;
                }
            }
        }
    }
}