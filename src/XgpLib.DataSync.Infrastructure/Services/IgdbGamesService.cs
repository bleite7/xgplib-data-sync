using System.Diagnostics;
using IGDB;

namespace XgpLib.DataSync.Infrastructure.Services;

public class IgdbGamesService(
    ILogger<IgdbPlatformsService> logger,
    IIgdbDataService igdbDataService,
    IMongoRepository<Game> gamesRepository) : IIgdbGamesService
{
    public async Task SyncIgdbGamesByPlatformAsync(long platformId, CancellationToken stoppingToken)
    {
        Stopwatch stopWatch = new();
        stopWatch.Start();

        try
        {
            List<IGDB.Models.Game> games = await igdbDataService.ListAllAsync<IGDB.Models.Game>(
                IGDBClient.Endpoints.Games,
                ["id", "name"],
                $"where platforms = [{platformId}];");

            foreach (var game in games)
            {
                await gamesRepository.ReplaceOneAsync(new Game(
                    game.Id,
                    game.Name), stoppingToken, true);

                logger.LogInformation(
                    "Game {id} {name}",
                    game.Id,
                    game.Name);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while syncing games");
            throw;
        }
        finally
        {
            stopWatch.Stop();
            logger.LogInformation(
                "{methodName} elapsed time: {elapsedTime} ms",
                nameof(SyncIgdbGamesByPlatformAsync),
                stopWatch.ElapsedMilliseconds);
        }
    }
}
