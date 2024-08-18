using System.Diagnostics;

namespace XgpLib.DataSync.Worker;

public class SyncWorker(
    ILogger<SyncWorker> logger,
    IConfiguration configuration,
    ISyncData syncData) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Stopwatch stopWatch = new();
        while (!stoppingToken.IsCancellationRequested)
        {
            stopWatch.Start();
            try
            {
                logger.LogInformation("{className} is starting.",
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
                    "{className} elapsed time: {elapsedTime}ms",
                    nameof(SyncWorker),
                    stopWatch.ElapsedMilliseconds);
                stopWatch.Reset();
            }

            double intervalInMinutes = double.Parse(configuration["SyncWorker:IntervalInMinutes"] ??= "0");
            await Task.Delay(TimeSpan.FromMinutes(intervalInMinutes), stoppingToken);
        }
    }
}
