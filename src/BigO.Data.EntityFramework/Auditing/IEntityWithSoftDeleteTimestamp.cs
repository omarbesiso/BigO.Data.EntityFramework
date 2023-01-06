namespace BigO.Data.EntityFramework.Auditing;

/// <summary>
///     Marks an entity (with soft delete support) with a timestamp log to when its record in the database was
///     marked as deleted.
/// </summary>
public interface IEntityWithSoftDeleteTimestamp
{
    /// <summary>
    ///     Gets or sets the the date and time the database record of the entity was soft deleted on.
    /// </summary>
    DateTime? DeletedOnUtc { get; set; }
}