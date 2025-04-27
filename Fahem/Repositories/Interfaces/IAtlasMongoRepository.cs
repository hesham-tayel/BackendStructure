using MongoDB.Driver;

namespace Fahem.Repositories.Interfaces
{
    public interface IMongoRepository
    {
        IMongoCollection<T> GetCollection<T>(string collectionName);

    }
}
