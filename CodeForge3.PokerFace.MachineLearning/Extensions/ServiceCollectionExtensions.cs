using CodeForge3.PokerFace.MachineLearning.Implementations;
using CodeForge3.PokerFace.MachineLearning.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CodeForge3.PokerFace.MachineLearning.Extensions;

/// <summary>
/// Provides extension methods for registering services related to machine learning
/// in the dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add the <see cref="IYoloDetectionHandler" /> to the dependency injection container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> instance.</param>
    /// <returns>The <see cref="IServiceCollection" /> instance for chaining.</returns>
    public static IServiceCollection AddYoloDetectionHandler(this IServiceCollection services)
    {
        services.AddScoped<IYoloDetectionHandler>(sp =>
            new YoloDetectionHandler(
                sp.GetRequiredService<ILogger<YoloDetectionHandler>>()
            )
        );
        return services;
    }
}
