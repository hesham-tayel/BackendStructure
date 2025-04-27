
using Fahem.Models;
using MongoDB.Driver;
using Fahem.Repositories.Interfaces;

namespace Fahem.Repositories.Orders
{
    public class OrderMongoRepository : IOrderMongoRepository
    {
        private readonly IMongoCollection<Order> _ordersCollection;

        public OrderMongoRepository(IMongoRepository atlasRepo)
        {
            var collectionName = Environment.GetEnvironmentVariable("ORDERS_COLLECTION_NAME") ?? "Orders";
            _ordersCollection = atlasRepo.GetCollection<Order>(collectionName);
        }

        public async Task InsertAsync(Order order, CancellationToken cancellationToken = default)
        {
            await _ordersCollection.InsertOneAsync(order, cancellationToken: cancellationToken);
        }
    }
}
