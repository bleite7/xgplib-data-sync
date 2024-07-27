using XgpLib.DataSync.Worker.Core.Domain.Services;

namespace XgpLib.DataSync.Worker;

public class SyncWorker(
    IHostApplicationLifetime hostApplicationLifetime,
    ILogger<SyncWorker> logger,
    IIgdbSyncService igdbSyncService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("SyncWorker is starting.");

        // Sync IGDB data
        await igdbSyncService.SyncIgdbDataAsync();

        // When completed, the entire app host will stop.
        hostApplicationLifetime.StopApplication();
    }
}
