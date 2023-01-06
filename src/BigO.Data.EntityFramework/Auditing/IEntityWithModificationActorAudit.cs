namespace BigO.Data.EntityFramework.Auditing;

/// <summary>
///     Marks an entity that tracks the actor (system or user) that modified the record.
/// </summary>
public interface IEntityWithModificationActorAudit<TActorId> where TActorId : struct
{
    /// <summary>
    ///     Gets or sets the identifier of the actor that last modified the record.
    /// </summary>
    TActorId? ModifiedBy { get; set; }
}