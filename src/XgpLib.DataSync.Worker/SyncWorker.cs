using XgpLib.DataSync.Worker.Core.Domain.Services;
using XgpLib.DataSync.Worker.Core.Domain.UseCases;

namespace XgpLib.DataSync.Worker;

public class SyncWorker(
    IHostApplicationLifetime hostApplicationLifetime,
    ILogger<SyncWorker> logger,
    ISyncData syncData) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("SyncWorker is starting.");

        // Sync IGDB data
        await syncData.SyncIgdbDataAsync();

        // When completed, the entire app host will stop.
        hostApplicationLifetime.StopApplication();
    }
}
