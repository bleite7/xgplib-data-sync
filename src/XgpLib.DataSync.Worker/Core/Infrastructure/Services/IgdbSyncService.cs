using System.Diagnostics;
using XgpLib.DataSync.Worker.Core.Domain.Services;

namespace XgpLib.DataSync.Worker.Core.Infrastructure.Services;

public class IgdbSyncService(
    ILogger<IgdbSyncService> logger,
    IIgdbGenresService igdbGenresService,
    IIgdbPlatformsService igdbPlatformsService,
    IIgdbGamesService igdbGamesService) : IIgdbSyncService
{
    private const long _xboxSeriesPlatformId = 169;

    public async Task SyncIgdbDataAsync()
    {
        Stopwatch stopWatch = new();
        stopWatch.Start();

        try
        {
            await igdbGenresService.SyncIgdbGenresAsync();
            await igdbPlatformsService.SyncIgdbPlatformsAsync();
            await igdbGamesService.SyncIgdbGamesByPlatformAsync(_xboxSeriesPlatformId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while syncing IGDB data");
            throw;
        }
        finally
        {
            stopWatch.Stop();
            logger.LogInformation(
                "IGDB data sync completed in {elapsed} ms",
                stopWatch.ElapsedMilliseconds);
        }
    }
}
