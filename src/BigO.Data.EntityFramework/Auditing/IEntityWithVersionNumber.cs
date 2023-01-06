namespace BigO.Data.EntityFramework.Auditing;

/// <summary>
///     Marks an entity with a version number for tracking versioned entities.
/// </summary>
public interface IEntityWithVersionNumber
{
    /// <summary>
    ///     Gets or sets the version number of the record including when it was deleted if soft delete is enabled.
    /// </summary>
    int VersionNumber { get; set; }
}