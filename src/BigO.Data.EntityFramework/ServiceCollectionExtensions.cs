using BigO.Core.Validation;
using BigO.Data.EntityFramework.Auditing;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace BigO.Data.EntityFramework;

[PublicAPI]
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Registers the type of <see cref="IActorIdProvider{TActorId}" /> to be sued when retrieving the identifier of the
    ///     actor used for auditing.
    /// </summary>
    /// <typeparam name="TActorProvider">The type of the actor provider.</typeparam>
    /// <typeparam name="TActorId">The type of the actor's identifier.</typeparam>
    /// <param name="serviceCollection">The service collection.</param>
    /// <param name="serviceLifetime">The service lifetime.</param>
    /// <returns>The same <see cref="IServiceCollection" /> so multiple calls can be chained.</returns>
    /// <exception cref="ArgumentOutOfRangeException">serviceLifetime - null</exception>
    public static IServiceCollection AddActorProvider<TActorProvider, TActorId>(
        this IServiceCollection serviceCollection,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where TActorProvider : class, IActorIdProvider<TActorId>
    {
        Guard.NotNull(serviceCollection, nameof(serviceCollection));

        switch (serviceLifetime)
        {
            case ServiceLifetime.Singleton:
                serviceCollection.AddSingleton<IActorIdProvider<TActorId>, TActorProvider>();
                break;
            case ServiceLifetime.Scoped:
                serviceCollection.AddScoped<IActorIdProvider<TActorId>, TActorProvider>();
                break;
            case ServiceLifetime.Transient:
                serviceCollection.AddTransient<IActorIdProvider<TActorId>, TActorProvider>();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(serviceLifetime), serviceLifetime, null);
        }

        return serviceCollection;
    }

    /// <summary>
    ///     Adds the registration of the <see cref="EfAuditInterceptor" />.
    /// </summary>
    /// <param name="serviceCollection">The service collection.</param>
    /// <param name="serviceLifetime">The service lifetime.</param>
    /// <returns>The same <see cref="IServiceCollection" /> so multiple calls can be chained.</returns>
    /// <exception cref="ArgumentOutOfRangeException">serviceLifetime - null</exception>
    public static IServiceCollection AddEfAuditInterceptor(
        this IServiceCollection serviceCollection,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
    {
        Guard.NotNull(serviceCollection, nameof(serviceCollection));

        switch (serviceLifetime)
        {
            case ServiceLifetime.Singleton:
                serviceCollection.AddSingleton<EfAuditInterceptor>();
                break;
            case ServiceLifetime.Scoped:
                serviceCollection.AddScoped<EfAuditInterceptor>();
                break;
            case ServiceLifetime.Transient:
                serviceCollection.AddTransient<EfAuditInterceptor>();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(serviceLifetime), serviceLifetime, null);
        }

        return serviceCollection;
    }

    /// <summary>
    ///     Adds the registration of the <see cref="EfAuditWithActorInterceptor{TActorId}" />.
    /// </summary>
    /// <typeparam name="TActorId">The type of the actor identifier.</typeparam>
    /// <param name="serviceCollection">The service collection.</param>
    /// <param name="serviceLifetime">The service lifetime.</param>
    /// <returns>The same <see cref="IServiceCollection" /> so multiple calls can be chained.</returns>
    /// <exception cref="ArgumentOutOfRangeException">serviceLifetime - null</exception>
    public static IServiceCollection AddEfAuditWithActorInterceptor<TActorId>(
        this IServiceCollection serviceCollection,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where TActorId : struct
    {
        Guard.NotNull(serviceCollection, nameof(serviceCollection));

        switch (serviceLifetime)
        {
            case ServiceLifetime.Singleton:
                serviceCollection.AddSingleton<EfAuditWithActorInterceptor<TActorId>>();
                break;
            case ServiceLifetime.Scoped:
                serviceCollection.AddScoped<EfAuditWithActorInterceptor<TActorId>>();
                break;
            case ServiceLifetime.Transient:
                serviceCollection.AddTransient<EfAuditWithActorInterceptor<TActorId>>();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(serviceLifetime), serviceLifetime, null);
        }

        return serviceCollection;
    }
}