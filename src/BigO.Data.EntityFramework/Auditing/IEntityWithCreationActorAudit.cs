namespace BigO.Data.EntityFramework.Auditing;

/// <summary>
///     Marks an entity that tracks the actor (system or user) that created the record.
/// </summary>
public interface IEntityWithCreationActorAudit<TActorId> where TActorId : struct
{
    /// <summary>
    ///     Gets or sets the identifier of the actor that created the record.
    /// </summary>
    TActorId CreatedBy { get; set; }
}