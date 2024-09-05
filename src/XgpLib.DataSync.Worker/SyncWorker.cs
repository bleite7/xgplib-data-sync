using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace XgpLib.DataSync.Worker;

[ExcludeFromCodeCoverage]
public class SyncWorker(
    ILogger<SyncWorker> logger,
    IConfiguration configuration,
    ISyncData syncData) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Stopwatch stopWatch = new();
            stopWatch.Start();
            try
            {
                logger.LogInformation(
                    "{ClassName} is starting.",
                    nameof(SyncWorker));

                // Sync IGDB data
                await syncData.SyncIgdbDataAsync(stoppingToken);

                logger.LogInformation("IGDB data synced successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while syncing data.");
            }
            finally
            {
                stopWatch.Stop();
                logger.LogInformation(
                    "{className} elapsed time: {ElapsedTime} ms",
                    nameof(SyncWorker),
                    stopWatch.ElapsedMilliseconds);
            }

            var intervalInMinutesFromConfig = configuration["SyncWorker:IntervalInMinutes"] ??= "0";
            await Task.Delay(TimeSpan.FromMinutes(double.Parse(intervalInMinutesFromConfig)), stoppingToken);
        }
    }
}
