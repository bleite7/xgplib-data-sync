using IGDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace XgpLib.DataSync.Infrastructure;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton(new IGDBClient(
            configuration["Igdb:ClientId"],
            configuration["Igdb:ClientSecret"]));

        // Register repositories
        services.AddTransient<IMongoRepository<Platform>, MongoRepository<Platform>>();
        services.AddTransient<IMongoRepository<Genre>, MongoRepository<Genre>>();
        services.AddTransient<IMongoRepository<Game>, MongoRepository<Game>>();

        // Register services
        services.AddTransient<IIgdbPlatformsService, IgdbPlatformsService>();
        services.AddTransient<IIgdbGenresService, IgdbGenresService>();
        services.AddTransient<IIgdbGamesService, IgdbGamesService>();

        // Register data services
        services.AddTransient<IIgdbDataService, IgdbDataService>();
        services.AddTransient<ISyncData, SyncData>();

        return services;
    }
}
