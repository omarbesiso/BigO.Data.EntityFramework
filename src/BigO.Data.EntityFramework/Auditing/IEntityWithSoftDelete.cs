namespace BigO.Data.EntityFramework.Auditing;

/// <summary>
///     Marks an entity who's records are to be marked as deleted instead of being physically deleted.
/// </summary>
public interface IEntityWithSoftDelete
{
    /// <summary>
    ///     Gets or sets a value indicating whether this instance is deleted.
    /// </summary>
    /// <value><c>true</c> if this instance is deleted; otherwise, <c>false</c>.</value>
    bool IsDeleted { get; set; }
}