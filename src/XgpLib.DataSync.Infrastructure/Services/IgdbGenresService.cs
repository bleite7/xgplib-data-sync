using System.Diagnostics;
using IGDB;

namespace XgpLib.DataSync.Infrastructure.Services;

public class IgdbGenresService(
    ILogger<IgdbGenresService> logger,
    IIgdbDataService igdbDataService,
    IMongoRepository<Genre> genresRepository) : IIgdbGenresService
{
    public async Task SyncIgdbGenresAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Syncing Genre(s)");

        List<IGDB.Models.Genre> genres = await igdbDataService.ListAllAsync<IGDB.Models.Genre>(
            IGDBClient.Endpoints.Genres,
            ["id", "name"]);

        foreach (var genre in genres)
        {
            await genresRepository.ReplaceOneAsync(new Genre(
                genre.Id,
                genre.Name), stoppingToken, true);
        }

        logger.LogInformation("Genre(s) synced successfully.");
    }
}
