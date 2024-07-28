using System.Diagnostics;

namespace XgpLib.DataSync.Worker;

public class SyncWorker(
    IHostApplicationLifetime hostApplicationLifetime,
    ILogger<SyncWorker> logger,
    ISyncData syncData) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Stopwatch stopWatch = new();
        stopWatch.Start();

        try
        {
            logger.LogInformation("SyncWorker is starting.");

            // Sync IGDB data
            await syncData.SyncIgdbDataAsync();

            // When completed, the entire app host will stop.
            hostApplicationLifetime.StopApplication();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while syncing data.");
        }
        finally
        {
            stopWatch.Stop();
            logger.LogInformation(
                "{methodName} elapsed time: {elapsedTime} ms",
                nameof(ExecuteAsync),
                stopWatch.ElapsedMilliseconds);
        }
    }
}
