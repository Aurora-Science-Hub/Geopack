using Microsoft.Extensions.DependencyInjection;

namespace AuroraScienceHub.Geopack;

/// <summary>
/// Extensions for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers Geopack services with the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to which Geopack services will be added.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddGeopack(this IServiceCollection services)
    {
        services.AddSingleton<IGeopack, Geopack>();

        return services;
    }
}
