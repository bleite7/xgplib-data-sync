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

        IMongoDatabase database = new MongoClient(_configuration["MongoDB:ConnectionString"]).GetDatabase(_configuration["MongoDB:DatabaseName"]);
        _collection = database.GetCollection<T>(typeof(T).Name.ToLower());
    }

    public async Task ReplaceOneAsync(T document, CancellationToken stoppingToken, bool IsUpsert = false)
    {
        FindOneAndReplaceOptions<T, T> options = new()
        {
            IsUpsert = IsUpsert
        };
        FilterDefinition<T> filter = Builders<T>.Filter.Eq(o => o.Id, document.Id);
        await _collection.FindOneAndReplaceAsync(filter, document, options, stoppingToken);
    }
}
