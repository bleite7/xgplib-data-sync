using IGDB;
using System.Diagnostics;

namespace XgpLib.DataSync.Infrastructure.Services;

public class IgdbDataService(
    ILogger<IgdbDataService> logger,
    IGDBClient igdbClient) : IIgdbDataService
{
    private const int _limit = 100;

    public async Task<List<T>> ListAllAsync<T>(
        string endpoint,
        string[] fields,
        string additionalQuery = "") where T : class
    {
        Stopwatch stopWatch = new();
        stopWatch.Start();

        int offset = 0;
        List<T> items = [];
        T[] igdbItems = [];
        try
        {
            logger.LogInformation(
                "Fetching {entityName}(s) from IGDB",
                typeof(T).Name);

            do
            {
                string query = BuildQuery(fields, offset, additionalQuery);
                igdbItems = await igdbClient.QueryAsync<T>(
                    endpoint,
                    query);

                items.AddRange(igdbItems);
                offset += _limit;
            } while (igdbItems.Length == _limit);

            logger.LogInformation(
                "Fetched {count} {entityName}(s) from IGDB",
                items.Count,
                typeof(T).Name);

            return items;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Error while fetching {entityName}(s) from IGDB",
                typeof(T).Name);

            throw;
        }
        finally
        {
            stopWatch.Stop();

            logger.LogInformation(
                "{methodName} of {entityName}(s) took {elapsed} ms",
                nameof(ListAllAsync),
                typeof(T).Name,
                stopWatch.ElapsedMilliseconds);
        }
    }

    #region Private Methods

    private string BuildQuery(
        string[] fields,
        int offset,
        string additionalQuery)
    {
        var baseQuery = $"fields {string.Join(',', fields)};offset {offset};limit {_limit};";
        return string.IsNullOrEmpty(additionalQuery) ? baseQuery : $"{baseQuery}{additionalQuery}";
    }

    #endregion
}
