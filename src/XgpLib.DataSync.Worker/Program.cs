using IGDB;
using IGDB.Models;
using Serilog;
using XgpLib.DataSync.Worker.Core.Domain.Repositories;
using XgpLib.DataSync.Worker.Core.Domain.Services;
using XgpLib.DataSync.Worker.Core.Infrastructure.Repositories;
using XgpLib.DataSync.Worker.Core.Infrastructure.Services;

namespace XgpLib.DataSync.Worker;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddSingleton(new IGDBClient(
                builder.Configuration["Igdb:ClientId"],
                builder.Configuration["Igdb:ClientSecret"]));
                
            // Register repositories
            builder.Services.AddTransient<IMongoRepository<Core.Domain.Genre>, MongoRepository<Core.Domain.Genre>>();
            builder.Services.AddTransient<IMongoRepository<Core.Domain.Platform>, MongoRepository<Core.Domain.Platform>>();
            builder.Services.AddTransient<IMongoRepository<Core.Domain.Game>, MongoRepository<Core.Domain.Game>>();

            // Register services
            builder.Services.AddTransient<IIgdbGenresService, IgdbGenresService>();
            builder.Services.AddTransient<IIgdbPlatformsService, IgdbPlatformsService>();
            builder.Services.AddTransient<IIgdbGamesService, IgdbGamesService>();

            // Register data services
            builder.Services.AddTransient<IIgdbDataService, IgdbDataService>();
            builder.Services.AddTransient<IIgdbSyncService, IgdbSyncService>();
            
            // Register worker
            builder.Services.AddHostedService<SyncWorker>();
            builder.Services.AddSerilog();

            var host = builder.Build();
            host.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
