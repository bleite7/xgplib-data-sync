using IGDB;
using IGDB.Models;
using System.Diagnostics;
using XgpLib.DataSync.Worker.Core.Domain.Services;

namespace XgpLib.DataSync.Worker.Core.Infrastructure.Services;

public class IgdbService(
    ILogger<IgdbService> logger,
    IGDBClient igdbClient) : IIgdbService
{
    private readonly int _limit = 50;

    public async Task<List<Genre>> ListAllGenres()
    {
        Stopwatch stopWatch = new();
        stopWatch.Start();

        var offset = 0;
        List<Genre> genres = [];
        Genre[] igdbGenres = [];
        try
        {
            do
            {
                igdbGenres = await igdbClient.QueryAsync<Genre>(
                    IGDBClient.Endpoints.Genres,
                    $"fields id, name; offset {offset}; limit {_limit};");

                genres.AddRange(igdbGenres);
                offset += _limit;
            } while (igdbGenres.Length == _limit);

            return genres;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while fetching genres from IGDB");
            throw;
        }
        finally
        {
            stopWatch.Stop();

            logger.LogInformation(
                "{methodName} took {elapsed} ms",
                nameof(ListAllGenres),
                stopWatch.ElapsedMilliseconds);
        }
    }
}
