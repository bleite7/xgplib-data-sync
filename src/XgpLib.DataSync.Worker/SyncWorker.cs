using System.Diagnostics;

namespace XgpLib.DataSync.Worker;

public class SyncWorker(
    ILogger<SyncWorker> logger,
    ISyncData syncData) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Stopwatch stopWatch = new();
        stopWatch.Start();

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                logger.LogInformation("SyncWorker is starting.");

                // Sync IGDB data
                await syncData.SyncIgdbDataAsync(stoppingToken);
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

            await Task.Delay(60_000, stoppingToken);
        }
    }
}
