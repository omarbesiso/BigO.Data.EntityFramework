namespace BigO.Data.EntityFramework.Auditing;

/// <summary>
///     Marks an entity with a timestamp logged for creating the record.
/// </summary>
public interface IEntityWithCreationTimeStamp
{
    /// <summary>
    ///     Gets or sets the the date and time the database record of the entity was created.
    /// </summary>
    DateTime CreatedOnUtc { get; set; }
}