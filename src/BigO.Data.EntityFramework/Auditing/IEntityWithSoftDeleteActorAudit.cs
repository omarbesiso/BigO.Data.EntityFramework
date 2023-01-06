namespace BigO.Data.EntityFramework.Auditing;

/// <summary>
///     Marks an entity (usually with soft delete support) with the actor who performed the soft delete.
/// </summary>
public interface IEntityWithSoftDeleteActorAudit<TActorId> where TActorId : struct
{
    /// <summary>
    ///     Gets or sets the identifier of the actor that soft deleted the record.
    /// </summary>
    TActorId? DeletedBy { get; set; }
}