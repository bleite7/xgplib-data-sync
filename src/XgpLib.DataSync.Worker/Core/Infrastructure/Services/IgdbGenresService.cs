using System.Diagnostics;
using IGDB;
using XgpLib.DataSync.Worker.Core.Domain.Repositories;
using XgpLib.DataSync.Worker.Core.Domain.Services;

namespace XgpLib.DataSync.Worker.Core.Infrastructure.Services;

public class IgdbGenresService(
    ILogger<IgdbGenresService> logger,
    IIgdbDataService igdbDataService,
    IMongoRepository<Domain.Genre> genresRepository) : IIgdbGenresService
{
    public async Task SyncIgdbGenresAsync()
    {
        Stopwatch stopWatch = new();
        stopWatch.Start();

        try
        {
            List<IGDB.Models.Genre> genres = await igdbDataService.ListAllAsync<IGDB.Models.Genre>(
                IGDBClient.Endpoints.Genres,
                ["id", "name"]);

            foreach (var genre in genres)
            {
                await genresRepository.ReplaceOneAsync(new Domain.Genre(
                    genre.Id,
                    genre.Name), true);

                logger.LogInformation(
                    "Genre {id} {name}",
                    genre.Id,
                    genre.Name);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while syncing genres");
            throw;
        }
        finally
        {
            stopWatch.Stop();
            logger.LogInformation(
                "{methodName} elapsed time: {elapsedTime}",
                nameof(SyncIgdbGenresAsync),
                stopWatch.Elapsed);
        }
    }
}
