using AuroraScienceHub.Geopack.ExternalFieldModels.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AuroraScienceHub.Geopack.ExternalFieldModels;

/// <summary>
/// Extensions for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExternalFieldModels(this IServiceCollection services)
    {
        services.AddSingleton<IT89, T89.T89>();

        return services;
    }
}
