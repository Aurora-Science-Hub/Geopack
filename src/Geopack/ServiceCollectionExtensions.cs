using Microsoft.Extensions.DependencyInjection;

namespace AuroraScienceHub.Geopack;

/// <summary>
/// Extensions for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add Geopack
    /// </summary>
    /// <param name="services">Service collection</param>
    public static IServiceCollection AddGeopack(this IServiceCollection services)
    {
        services.AddSingleton<IGeopack, Geopack>();

        return services;
    }
}
