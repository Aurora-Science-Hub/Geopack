using Microsoft.Extensions.DependencyInjection;

namespace AuroraScienceHub.Geopack;

/// <summary>
/// Extensions for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGeopack(this IServiceCollection services)
    {
        services.AddSingleton<IGeopack, Geopack>();

        return services;
    }
}
