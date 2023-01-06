namespace BigO.Data.EntityFramework.Auditing;

/// <summary>
///     Contract for retrieving the the identifier of the actor performing data operations.
/// </summary>
/// <typeparam name="TActorId">The type of the actor's identifier.</typeparam>
public interface IActorIdProvider<TActorId>
{
    /// <summary>
    ///     Gets the actor's identifier.
    /// </summary>
    /// <returns>The actor's identifier to be sued for auditing.</returns>
    Task<TActorId> GetActorId();
}