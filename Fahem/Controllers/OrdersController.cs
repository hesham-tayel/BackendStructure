using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Fahem.Commands.Orders.CreateOrder;
using Fahem.Dtos.OrdersDtos;

namespace Fahem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(
            [FromBody] CreateOrderDto dto,
            CancellationToken cancellationToken)
        {
            var command = new CreateOrderCommand { CreateOrderDto = dto };
            var orderId = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(nameof(GetOrderById), new { id = orderId }, new { OrderId = orderId });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(string id, CancellationToken cancellationToken)
        {
            return Ok();
        }
    }
}
