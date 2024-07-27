using System.Runtime.CompilerServices;
using IGDB;
using Serilog;
using XgpLib.DataSync.Worker.Core.Domain.Repositories;
using XgpLib.DataSync.Worker.Core.Domain.Services;
using XgpLib.DataSync.Worker.Core.Infrastructure;
using XgpLib.DataSync.Worker.Core.Infrastructure.Repositories;
using XgpLib.DataSync.Worker.Core.Infrastructure.Services;

namespace XgpLib.DataSync.Worker;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            // Register Serilog
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            // Create the host builder
            var builder = Host.CreateApplicationBuilder(args);

            // Add infrastructure services
            DependencyInjection.AddInfrastructureServices(
                builder.Services,
                builder.Configuration);

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
