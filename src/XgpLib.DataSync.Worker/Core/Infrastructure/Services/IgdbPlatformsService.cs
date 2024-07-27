using System.Diagnostics;
using IGDB;
using XgpLib.DataSync.Worker.Core.Domain.Repositories;
using XgpLib.DataSync.Worker.Core.Domain.Services;

namespace XgpLib.DataSync.Worker.Core.Infrastructure.Services;

public class IgdbPlatformsService(
    ILogger<IgdbPlatformsService> logger,
    IIgdbDataService igdbDataService,
    IMongoRepository<Domain.Platform> platformsRepository) : IIgdbPlatformsService
{
    public async Task SyncIgdbPlatformsAsync()
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
                await platformsRepository.ReplaceOneAsync(new Domain.Platform(
                    platform.Id,
                    platform.Name), true);

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
                "{methodName} elapsed time: {elapsedTime}",
                nameof(SyncIgdbPlatformsAsync),
                stopWatch.Elapsed);
        }
    }
}
