using BigO.Core.Validation;
using BigO.Data.EntityFramework.Auditing;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BigO.Data.EntityFramework;

/// <summary>
///     Useful helpers and extensions for using with <see cref="DbContextOptionsBuilder" />.
/// </summary>
[PublicAPI]
public static class DbContextOptionsBuilderExtensions
{
    /// <summary>
    ///     Adds the <see cref="EfAuditInterceptor" /> to the <see cref="DbContextOptionsBuilder" /> instance.
    /// </summary>
    /// <param name="optionsBuilder">The options builder.</param>
    /// <returns>The same <see cref="DbContextOptionsBuilder" /> so multiple calls can be chained.</returns>
    public static DbContextOptionsBuilder AddEfAuditInterceptor(this DbContextOptionsBuilder optionsBuilder)
    {
        Guard.NotNull(optionsBuilder, nameof(optionsBuilder));
        return optionsBuilder.AddInterceptors(new EfAuditInterceptor());
    }

    /// <summary>
    ///     Adds the <see cref="EfAuditWithActorInterceptor{TActorId}" /> to the <see cref="DbContextOptionsBuilder" />
    ///     instance.
    /// </summary>
    /// <typeparam name="TActorId">The type of the actor's identifier.</typeparam>
    /// <param name="optionsBuilder">The options builder.</param>
    /// <param name="provider">
    ///     The service provider to be used to resolve the
    ///     <see cref="EfAuditWithActorInterceptor{TActorId}" /> instance.
    /// </param>
    /// <returns>The same <see cref="DbContextOptionsBuilder" /> so multiple calls can be chained.</returns>
    public static DbContextOptionsBuilder AddEfAuditWithActorInterceptor<TActorId>(
        this DbContextOptionsBuilder optionsBuilder, IServiceProvider provider)
    {
        Guard.NotNull(optionsBuilder, nameof(optionsBuilder));
        return optionsBuilder.AddInterceptors(provider.GetRequiredService<EfAuditWithActorInterceptor<TActorId>>());
    }
}