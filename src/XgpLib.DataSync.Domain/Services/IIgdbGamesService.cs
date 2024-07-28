namespace XgpLib.DataSync.Domain.Services;

public interface IIgdbGamesService
{
    Task SyncIgdbGamesByPlatformAsync(long platformId, CancellationToken stoppingToken);
}
