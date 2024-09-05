using IGDB;

namespace XgpLib.DataSync.Infrastructure.Services;

public class IgdbDataService(
    ILogger<IgdbDataService> logger,
    IGDBClient igdbClient) : IIgdbDataService
{
    private const int _limit = 200;

    public async Task<List<T>> ListAllAsync<T>(
        string endpoint,
        string[] fields,
        string additionalQuery = "") where T : class
    {
        int offset = 0;
        List<T> items = [];
        T[] igdbItems;

        logger.LogInformation(
            "Fetching {EntityName}(s) from IGDB",
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
            "Fetched {Count} {EntityName}(s) from IGDB",
            items.Count,
            typeof(T).Name);

        return items;
    }

    #region Private Methods

    private static string BuildQuery(
        string[] fields,
        int offset,
        string additionalQuery)
    {
        var baseQuery = $"fields {string.Join(',', fields)};offset {offset};limit {_limit};";
        return string.IsNullOrEmpty(additionalQuery) ? baseQuery : $"{baseQuery}{additionalQuery}";
    }

    #endregion
}
