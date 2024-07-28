using System.Diagnostics;

namespace XgpLib.DataSync.Application.UseCases;

public class SyncData(
    ILogger<SyncData> logger,
    IIgdbGenresService igdbGenresService,
    IIgdbPlatformsService igdbPlatformsService,
    IIgdbGamesService igdbGamesService) : ISyncData
{
    private const long _xboxSeriesPlatformId = 169;

    public async Task SyncIgdbDataAsync(CancellationToken stoppingToken)
    {
        Stopwatch stopWatch = new();
        stopWatch.Start();

        try
        {
            await igdbGenresService.SyncIgdbGenresAsync(stoppingToken);
            await igdbPlatformsService.SyncIgdbPlatformsAsync(stoppingToken);
            await igdbGamesService.SyncIgdbGamesByPlatformAsync(_xboxSeriesPlatformId, stoppingToken);
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
