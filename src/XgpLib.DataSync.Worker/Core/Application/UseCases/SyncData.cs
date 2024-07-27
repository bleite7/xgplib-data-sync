using System.Diagnostics;
using XgpLib.DataSync.Worker.Core.Domain.Services;
using XgpLib.DataSync.Worker.Core.Domain.UseCases;

namespace XgpLib.DataSync.Worker.Core.Application.UseCases;

public class SyncData(
    ILogger<SyncData> logger,
    IIgdbGenresService igdbGenresService,
    IIgdbPlatformsService igdbPlatformsService,
    IIgdbGamesService igdbGamesService) : ISyncData
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
                "{methodName} completed in {elapsed} ms",
                nameof(SyncIgdbDataAsync),
                stopWatch.ElapsedMilliseconds);
        }
    }
}
