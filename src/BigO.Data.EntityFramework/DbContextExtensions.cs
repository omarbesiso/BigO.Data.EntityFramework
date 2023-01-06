using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace BigO.Data.EntityFramework;

[PublicAPI]
public static class DbContextExtensions
{
    /// <summary>
    ///     Determines if the specified <see cref="DbContext" /> has any changes.
    /// </summary>
    /// <param name="context">The <see cref="DbContext" /> to check for changes.</param>
    /// <returns>True if the <paramref name="context" /> has any changes, false otherwise.</returns>
    /// <remarks>
    ///     This method checks the <see cref="DbContext.ChangeTracker" /> for any entries that have a state of
    ///     <see cref="EntityState.Added" />, <see cref="EntityState.Modified" />, or <see cref="EntityState.Deleted" />.
    ///     If any such entries are found, the method returns true. Otherwise, it returns false.
    /// </remarks>
    public static bool HasChanges(this DbContext context)
    {
        return context.ChangeTracker.Entries().Any(e =>
            e.State is EntityState.Added or
                EntityState.Modified or
                EntityState.Deleted);
    }
}