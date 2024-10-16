using IGDB;

namespace XgpLib.DataSync.Infrastructure.Services;

public class IgdbPlatformsService(
    ILogger<IgdbPlatformsService> logger,
    IIgdbDataService igdbDataService,
    IMongoRepository<Platform> platformsRepository) : IIgdbPlatformsService
{
    public async Task SyncIgdbPlatformsAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Syncing Platform(s)");

        List<IGDB.Models.Platform> platforms = await igdbDataService.ListAllAsync<IGDB.Models.Platform>(
            IGDBClient.Endpoints.Platforms,
            ["id", "name"]);

        foreach (var platform in platforms)
        {
            await platformsRepository.ReplaceOneAsync(new Platform(
                platform.Id,
                platform.Name), stoppingToken, true);
        }

        logger.LogInformation("Platform(s) synced successfully.");
    }
}
