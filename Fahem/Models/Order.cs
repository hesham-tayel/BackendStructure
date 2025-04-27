using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Fahem.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string OrderId { get; set; }

        public double Price { get; set; }

        public string CustomerName { get; set; }

        public string Address { get; set; }
    }
}
