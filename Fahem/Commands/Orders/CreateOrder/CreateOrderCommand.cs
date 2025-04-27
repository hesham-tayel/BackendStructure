using Fahem.Dtos.OrdersDtos;
using MediatR;
namespace Fahem.Commands.Orders.CreateOrder
{

    public class CreateOrderCommand : IRequest<string>
    {
        public CreateOrderDto CreateOrderDto { get; set; }
    }


}
