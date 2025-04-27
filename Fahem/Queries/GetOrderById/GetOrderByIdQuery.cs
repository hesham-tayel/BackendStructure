using Fahem.Models;
using MediatR;

namespace Fahem.Queries.GetOrderById
{
    public class GetOrderByIdQuery :IRequest<Order>
    {
        public GetOrderByIdQuery(string id) => OrderId = id;
        
        public string OrderId { get; set; }
    }
}
