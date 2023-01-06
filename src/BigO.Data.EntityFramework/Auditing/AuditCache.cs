using BigO.Core.Validation;

namespace BigO.Data.EntityFramework.Auditing;

internal static class AuditCache
{
    private static readonly Dictionary<string, EntityAuditSupport> EntityAuditMap =
        new();

    public static EntityAuditSupport GetEntityAuditSupport(Type entityType)
    {
        Guard.NotNull(entityType, nameof(entityType));

        var typeName = entityType.FullName;

        if (typeName == null)
        {
            throw new NullReferenceException($"The full name of type {entityType.Name} cannot be null.");
        }

        if (EntityAuditMap.ContainsKey(typeName))
        {
            return EntityAuditMap[typeName];
        }

        var supportsCreationTimestamp = entityType.IsAssignableTo(typeof(IEntityWithCreationTimeStamp));
        var supportsModificationTimestamp = entityType.IsAssignableTo(typeof(IEntityWithModificationTimeStamp));
        var supportsSoftDeleteTimestamps = entityType.IsAssignableTo(typeof(IEntityWithSoftDeleteTimestamp));

        var supportsSoftDelete = entityType.IsAssignableTo(typeof(IEntityWithSoftDelete));

        var supportsCreationActorAudits = entityType.GetInterfaces().Any(CheckCreationActorAudit);
        var supportsModificationActorAudits = entityType.GetInterfaces().Any(CheckModificationActorAudit);
        var supportsSoftDeletionActorAudits = entityType.GetInterfaces().Any(CheckDeleteActorAudit);

        var supportsVersioning = entityType.IsAssignableTo(typeof(IEntityWithVersionNumber));

        var entityAuditSupport = new EntityAuditSupport(entityType, supportsCreationTimestamp,
            supportsModificationTimestamp, supportsCreationActorAudits, supportsModificationActorAudits,
            supportsSoftDelete, supportsSoftDeleteTimestamps, supportsSoftDeletionActorAudits, supportsVersioning);

        EntityAuditMap.Add(typeName, entityAuditSupport);

        return new EntityAuditSupport();
    }

    private static bool CheckCreationActorAudit(Type x)
    {
        return x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEntityWithCreationActorAudit<>);
    }

    private static bool CheckModificationActorAudit(Type x)
    {
        return x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEntityWithModificationActorAudit<>);
    }

    private static bool CheckDeleteActorAudit(Type x)
    {
        return x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEntityWithSoftDeleteActorAudit<>);
    }
}