namespace XgpLib.DataSync.Worker.Core.Domain.Repositories;

public interface IMongoRepository<T> where T : IDocument
{
    Task ReplaceOneAsync(T document);
}
