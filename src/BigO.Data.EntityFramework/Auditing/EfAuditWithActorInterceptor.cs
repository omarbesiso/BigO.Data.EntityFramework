using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BigO.Data.EntityFramework.Auditing;

/// <summary>
///     Custom interceptor that interrogates entries to be saved into a database and applying relevant timestamps
///     AND ACTOR identifiers for supported entities that inherit from the relevant auditing interfaces.
///     Implements the <see cref="Microsoft.EntityFrameworkCore.Diagnostics.SaveChangesInterceptor" />
/// </summary>
/// <seealso cref="Microsoft.EntityFrameworkCore.Diagnostics.SaveChangesInterceptor" />
public class EfAuditWithActorInterceptor<TActorId> : SaveChangesInterceptor
{
    private readonly IActorIdProvider<TActorId?> _actorIdProvider;

    /// <summary>
    ///     Initializes a new instance of the <see cref="EfAuditWithActorInterceptor{TActorId}" /> class.
    /// </summary>
    /// <param name="actorIdProvider">The actor identifier provider.</param>
    public EfAuditWithActorInterceptor(IActorIdProvider<TActorId?> actorIdProvider)
    {
        _actorIdProvider = actorIdProvider;
    }

    /// <summary>
    ///     Called at the start of <see cref="M:DbContext.SaveChanges" />.
    /// </summary>
    /// <param name="eventData">
    ///     Contextual information about the <see cref="T:Microsoft.EntityFrameworkCore.DbContext" /> being
    ///     used.
    /// </param>
    /// <param name="result">
    ///     Represents the current result if one exists.
    ///     This value will have <see cref="P:Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult`1.HasResult" /> set
    ///     to <see langword="true" /> if some previous interceptor suppressed execution by calling
    ///     <see cref="M:Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult`1.SuppressWithResult(`0)" />.
    ///     This value is typically used as the return value for the implementation of this method.
    /// </param>
    /// <returns>
    ///     If <see cref="P:Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult`1.HasResult" /> is false, the EF will
    ///     continue as normal.
    ///     If <see cref="P:Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult`1.HasResult" /> is true, then EF will
    ///     suppress the operation it
    ///     was about to perform and use <see cref="P:Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult`1.Result" />
    ///     instead. A normal implementation of this method for any interceptor that is not attempting to change the result
    ///     is to return the <paramref name="result" /> value passed in.
    /// </returns>
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        eventData.Context.AuditEntries(_actorIdProvider.GetActorId());
        return base.SavingChanges(eventData, result);
    }

    /// <summary>
    ///     Called at the start of <see cref="M:DbContext.SaveChangesAsync" />.
    /// </summary>
    /// <param name="eventData">
    ///     Contextual information about the <see cref="T:Microsoft.EntityFrameworkCore.DbContext" /> being
    ///     used.
    /// </param>
    /// <param name="result">
    ///     Represents the current result if one exists.
    ///     This value will have <see cref="P:Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult`1.HasResult" /> set
    ///     to <see langword="true" /> if some previous
    ///     interceptor suppressed execution by calling
    ///     <see cref="M:Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult`1.SuppressWithResult(`0)" />.
    ///     This value is typically used as the return value for the implementation of this method.
    /// </param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///     If <see cref="P:Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult`1.HasResult" /> is false, the EF will
    ///     continue as normal.
    ///     If <see cref="P:Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult`1.HasResult" /> is true, then EF will
    ///     suppress the operation it
    ///     was about to perform and use <see cref="P:Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult`1.Result" />
    ///     instead.
    ///     A normal implementation of this method for any interceptor that is not attempting to change the result
    ///     is to return the <paramref name="result" /> value passed in.
    /// </returns>
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        eventData.Context.AuditEntries(_actorIdProvider.GetActorId());
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}