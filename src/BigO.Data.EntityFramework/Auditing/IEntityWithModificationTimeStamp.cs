namespace BigO.Data.EntityFramework.Auditing;

/// <summary>
///     Marks an entity with a timestamp logged for modifying the record.
/// </summary>
public interface IEntityWithModificationTimeStamp
{
    /// <summary>
    ///     Gets or sets the the date and time the database record of the entity was last modified.
    /// </summary>
    DateTime? ModifiedOnUtc { get; set; }
}