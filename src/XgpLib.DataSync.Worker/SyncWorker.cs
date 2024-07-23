using XgpLib.DataSync.Worker.Core.Domain.Services;

namespace XgpLib.DataSync.Worker;

public class SyncWorker(
    IHostApplicationLifetime hostApplicationLifetime,
    ILogger<SyncWorker> logger,
    IIgdbSyncService igdbSyncService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await igdbSyncService.SyncIgdbDataAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while syncing IGDB data");
            throw;
        }

        // When completed, the entire app host will stop.
        hostApplicationLifetime.StopApplication();
    }
}
