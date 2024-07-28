using System.Diagnostics;
using IGDB;

namespace XgpLib.DataSync.Infrastructure.Services;

public class IgdbPlatformsService(
    ILogger<IgdbPlatformsService> logger,
    IIgdbDataService igdbDataService,
    IMongoRepository<Platform> platformsRepository) : IIgdbPlatformsService
{
    public async Task SyncIgdbPlatformsAsync(CancellationToken stoppingToken)
    {
        Stopwatch stopWatch = new();
        stopWatch.Start();

        try
        {
            List<IGDB.Models.Platform> platforms = await igdbDataService.ListAllAsync<IGDB.Models.Platform>(
                IGDBClient.Endpoints.Platforms,
                ["id", "name"]);

            foreach (var platform in platforms)
            {
                await platformsRepository.ReplaceOneAsync(new Platform(
                    platform.Id,
                    platform.Name), stoppingToken, true);

                logger.LogInformation(
                    "Platform {id} {name}",
                    platform.Id,
                    platform.Name);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while syncing platforms");
            throw;
        }
        finally
        {
            stopWatch.Stop();
            logger.LogInformation(
                "{methodName} elapsed time: {elapsedTime} ms",
                nameof(SyncIgdbPlatformsAsync),
                stopWatch.ElapsedMilliseconds);
        }
    }
}
