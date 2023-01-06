namespace BigO.Data.EntityFramework.Auditing;

internal readonly struct EntityAuditSupport
{
    public EntityAuditSupport(Type entityType, bool supportsCreationTimestamp, bool supportsModificationTimestamp,
        bool supportsCreationActorAudits, bool supportsModificationActorAudits, bool supportsSoftDelete,
        bool supportsSoftDeleteTimestamps, bool supportSoftDeleteActorAudits, bool supportsVersioning)
    {
        EntityType = entityType;
        SupportsCreationTimestamp = supportsCreationTimestamp;
        SupportsModificationTimestamp = supportsModificationTimestamp;
        SupportsCreationActorAudits = supportsCreationActorAudits;
        SupportsModificationActorAudits = supportsModificationActorAudits;
        SupportsSoftDelete = supportsSoftDelete;
        SupportsSoftDeleteTimestamps = supportsSoftDeleteTimestamps;
        SupportSoftDeleteActorAudits = supportSoftDeleteActorAudits;
        SupportsVersioning = supportsVersioning;
    }

    public Type EntityType { get; }
    public bool SupportsCreationTimestamp { get; }
    public bool SupportsModificationTimestamp { get; }
    public bool SupportsCreationActorAudits { get; }
    public bool SupportsModificationActorAudits { get; }
    public bool SupportsSoftDelete { get; }
    public bool SupportsSoftDeleteTimestamps { get; }
    public bool SupportSoftDeleteActorAudits { get; }
    public bool SupportsVersioning { get; }
}