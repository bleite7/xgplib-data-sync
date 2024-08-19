namespace XgpLib.DataSync.Application.UseCases;

public class SyncData(
    IIgdbGamesService igdbGamesService,
    IIgdbGenresService igdbGenresService,
    IIgdbPlatformsService igdbPlatformsService) : ISyncData
{
    private const long _xboxSeriesPlatformId = 169;

    public async Task SyncIgdbDataAsync(CancellationToken stoppingToken)
    {
        await igdbGamesService.SyncIgdbGamesByPlatformAsync(_xboxSeriesPlatformId, stoppingToken);
        await igdbGenresService.SyncIgdbGenresAsync(stoppingToken);
        await igdbPlatformsService.SyncIgdbPlatformsAsync(stoppingToken);
    }
}
