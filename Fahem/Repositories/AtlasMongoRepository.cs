using Fahem.Repositories.Interfaces;
using Fahem.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Fahem.Repositories
{
    public class AtlasMongoRepository : IMongoRepository
    {
        private readonly IMongoDatabase _database;

        public AtlasMongoRepository(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }
}
