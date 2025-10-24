using CodeForge3.PokerFace.Services.Implementations;
using CodeForge3.PokerFace.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CodeForge3.PokerFace.Services.Extensions;

/// <summary>
/// Provides extension methods for registering services related to the application
/// in the dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add the <see cref="IPokerAppService" /> to the dependency injection container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> instance.</param>
    /// <returns>The <see cref="IServiceCollection" /> instance for chaining.</returns>
    public static IServiceCollection AddPokerAppService(this IServiceCollection services)
    {
        services.AddScoped<IPokerAppService, PokerAppService>();
        return services;
    }
    public static IServiceCollection AddPokerHandEvaluatorService(this IServiceCollection services)
    {
        services.AddScoped<IPokerCombinationEvaluatorService, PokerCombinationEvaluatorService>();
        return services;
    }
}
