using IGDB;
using Serilog;
using XgpLib.DataSync.Worker.Core.Application.UseCases;
using XgpLib.DataSync.Worker.Core.Domain.Repositories;
using XgpLib.DataSync.Worker.Core.Domain.Services;
using XgpLib.DataSync.Worker.Core.Domain.UseCases;
using XgpLib.DataSync.Worker.Core.Infrastructure.Repositories;
using XgpLib.DataSync.Worker.Core.Infrastructure.Services;

namespace XgpLib.DataSync.Worker.Core.Infrastructure;

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
        services.AddTransient<IMongoRepository<Domain.Platform>, MongoRepository<Domain.Platform>>();
        services.AddTransient<IMongoRepository<Domain.Genre>, MongoRepository<Domain.Genre>>();
        services.AddTransient<IMongoRepository<Domain.Game>, MongoRepository<Domain.Game>>();

        // Register services
        services.AddTransient<IIgdbPlatformsService, IgdbPlatformsService>();
        services.AddTransient<IIgdbGenresService, IgdbGenresService>();
        services.AddTransient<IIgdbGamesService, IgdbGamesService>();

        // Register data services
        services.AddTransient<IIgdbDataService, IgdbDataService>();
        services.AddTransient<ISyncData, SyncData>();

        // Register worker
        services.AddHostedService<SyncWorker>();
        services.AddSerilog();

        return services;
    }
}
