using AuroraScienceHub.Geopack.ExternalFieldModels.T89;
using Microsoft.Extensions.DependencyInjection;

namespace AuroraScienceHub.Geopack.ExternalFieldModels;

/// <summary>
/// Extensions for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add External field models
    /// </summary>
    /// <param name="services">Service collection</param>
    public static IServiceCollection AddExternalFieldModels(this IServiceCollection services)
    {
        services.AddSingleton<IT89, T89.T89>();

        return services;
    }
}
