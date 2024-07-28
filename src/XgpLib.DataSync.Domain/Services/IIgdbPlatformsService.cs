namespace XgpLib.DataSync.Domain.Services;

public interface IIgdbPlatformsService
{
    Task SyncIgdbPlatformsAsync(CancellationToken stoppingToken);
}
