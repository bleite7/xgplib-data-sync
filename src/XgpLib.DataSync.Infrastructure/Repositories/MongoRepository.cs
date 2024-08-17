using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace XgpLib.DataSync.Infrastructure.Services;

public class MongoRepository<T> :
    IMongoRepository<T> where T : IBaseEntity
{
    private readonly IConfiguration _configuration;
    private readonly IMongoCollection<T> _collection;

    public MongoRepository(IConfiguration configuration)
    {
        _configuration = configuration;

        MongoUrl mongoUrl = new(@_configuration["MongoDB:ConnectionString"]);
        IMongoDatabase database = new MongoClient(mongoUrl).GetDatabase(mongoUrl.DatabaseName);
        _collection = database.GetCollection<T>(typeof(T).Name.ToLower());
    }

    public async Task ReplaceOneAsync(T document, CancellationToken stoppingToken, bool isUpsert = false)
    {
        FindOneAndReplaceOptions<T, T> options = new()
        {
            IsUpsert = isUpsert
        };
        FilterDefinition<T> filter = Builders<T>.Filter.Eq(o => o.Id, document.Id);
        await _collection.FindOneAndReplaceAsync(filter, document, options, stoppingToken);
    }
}
