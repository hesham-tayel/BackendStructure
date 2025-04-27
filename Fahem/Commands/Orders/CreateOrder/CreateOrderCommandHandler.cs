using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Fahem.Models;
using Fahem.Repositories.Orders;
using Fahem.Dtos;

namespace Fahem.Commands.Orders.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, string>
    {
        private readonly IOrderMongoRepository _orderRepository;
        private readonly IMapper _mapper;

        public CreateOrderCommandHandler(IOrderMongoRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<string> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = _mapper.Map<Order>(request.CreateOrderDto);

            await _orderRepository.InsertAsync(order, cancellationToken);

            return order.OrderId;
        }
    }
}
