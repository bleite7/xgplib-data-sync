using IGDB;
using XgpLib.DataSync.Worker.Core.Domain.Repositories;
using XgpLib.DataSync.Worker.Core.Domain.Services;

namespace XgpLib.DataSync.Worker.Core.Infrastructure.Services;

public class IgdbGamesService(
    ILogger<IgdbPlatformsService> logger,
    IIgdbDataService igdbDataService,
    IMongoRepository<Domain.Game> gamesRepository) : IIgdbGamesService
{
    public async Task SyncIgdbGamesByPlatformAsync(long platformId)
    {
        try
        {
            List<IGDB.Models.Game> games = await igdbDataService.ListAllAsync<IGDB.Models.Game>(
                IGDBClient.Endpoints.Games,
                ["id", "name"],
                $"where platforms = [{platformId}];");

            foreach (var game in games)
            {
                await gamesRepository.ReplaceOneAsync(new Domain.Game(
                    game.Id,
                    game.Name));

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
    }
}
