using MediatR;
using Microsoft.AspNetCore.Mvc;
using Yakihouse.Application.Orders.Commands.CreateOrder;
using Yakihouse.Application.Orders.Commands.UpdateOrderStatus;
using Yakihouse.Application.Orders.Commands.UpgradeBuffetType;
using Yakihouse.Application.Orders.Queries.GetActiveOrders;
using Yakihouse.Application.Orders.Queries.GetOrderById;

namespace Yakihouse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetActiveOrders(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetActiveOrdersQuery(), cancellationToken);
        
        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetOrderByIdQuery(id), cancellationToken);
        
        if (!result.IsSuccess)
            return NotFound(result.Error);

        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        
        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetOrderById), new { id = result.Data!.Id }, result.Data);
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] UpdateOrderStatusRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateOrderStatusCommand
        {
            OrderId = id,
            Status = request.Status,
            ActorId = request.ActorId
        };

        var result = await _mediator.Send(command, cancellationToken);
        
        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return NoContent();
    }

    [HttpPost("{id}/upgrade-buffet")]
    public async Task<IActionResult> UpgradeBuffetType(Guid id, [FromBody] UpgradeBuffetTypeRequest request, CancellationToken cancellationToken)
    {
        var command = new UpgradeBuffetTypeCommand
        {
            OrderId = id,
            ActorId = request.ActorId
        };

        var result = await _mediator.Send(command, cancellationToken);
        
        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return NoContent();
    }
}

public record UpdateOrderStatusRequest(string Status, Guid ActorId);
public record UpgradeBuffetTypeRequest(Guid ActorId);

