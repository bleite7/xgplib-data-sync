using IGDB;

namespace XgpLib.DataSync.Infrastructure.Services;

public class IgdbGamesService(
    ILogger<IgdbGamesService> logger,
    IIgdbDataService igdbDataService,
    IMongoRepository<Game> gamesRepository) : IIgdbGamesService
{
    public async Task SyncIgdbGamesByPlatformAsync(long platformId, CancellationToken stoppingToken)
    {
        logger.LogInformation("Syncing Game(s) for platform {platformId}", platformId);

        List<IGDB.Models.Game> games = await igdbDataService.ListAllAsync<IGDB.Models.Game>(
            IGDBClient.Endpoints.Games,
            ["id", "name"],
            $"where platforms = [{platformId}];");

        foreach (var game in games)
        {
            await gamesRepository.ReplaceOneAsync(new Game(
                game.Id,
                game.Name), stoppingToken, true);
        }

        logger.LogInformation("Game(s) for platform {platformId} synced successfully.", platformId);
    }
}
