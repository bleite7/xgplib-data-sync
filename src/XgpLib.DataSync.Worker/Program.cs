using IGDB;
using Serilog;
using XgpLib.DataSync.Worker.Core.Domain.Services;
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

            builder.Services.AddTransient<IIgdbService, IgdbService>();
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
