using Serilog;
using Serilog.Events;

namespace XgpLib.DataSync.Worker;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            // Create the host builder
            var builder = Host.CreateApplicationBuilder(args);

            // Register Serilog
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                // Write logs to MongoDB
                .WriteTo.MongoDBBson(
                    $"{builder.Configuration["MongoDB:ConnectionString"]}/logs?authSource=admin",
                    restrictedToMinimumLevel: LogEventLevel.Warning)
                .WriteTo.Console()
                .CreateLogger();

            // Add infrastructure services
            DependencyInjection.AddInfrastructureServices(
                builder.Services,
                builder.Configuration);

            // Register worker
            builder.Services.AddHostedService<SyncWorker>();
            builder.Services.AddSerilog();

            // Build and run the host
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
