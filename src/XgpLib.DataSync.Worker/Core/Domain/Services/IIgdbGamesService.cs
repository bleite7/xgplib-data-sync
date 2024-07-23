namespace XgpLib.DataSync.Worker.Core.Domain.Services;

public interface IIgdbGamesService
{
    Task SyncIgdbGamesByPlatformAsync(long platformId);
}
