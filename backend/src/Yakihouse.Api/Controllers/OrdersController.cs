using Microsoft.AspNetCore.Mvc;
using Yakihouse.Application.Orders.Commands.CreateOrder;
using Yakihouse.Application.Orders.Commands.UpdateOrderStatus;
using Yakihouse.Application.Orders.Commands.UpgradeBuffetType;
using Yakihouse.Application.Orders.DTOs;
using Yakihouse.Application.Orders.Queries.GetActiveOrders;
using Yakihouse.Application.Orders.Queries.GetOrderById;

namespace Yakihouse.Api.Controllers;

/// <summary>
/// Orders management endpoints
/// </summary>
public class OrdersController : BaseApiController
{
    /// <summary>
    /// Get all active orders
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of active orders</returns>
    /// <response code="200">Returns the list of active orders</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<OrderDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActiveOrders(CancellationToken cancellationToken)
    {
        Logger.LogInformation("Getting active orders");
        var result = await Mediator.Send(new GetActiveOrdersQuery(), cancellationToken);
        return OkResult(result);
    }

    /// <summary>
    /// Get order by ID
    /// </summary>
    /// <param name="id">Order ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Order details</returns>
    /// <response code="200">Returns the order details</response>
    /// <response code="404">Order not found</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrderById(Guid id, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Getting order {OrderId}", id);
        var result = await Mediator.Send(new GetOrderByIdQuery(id), cancellationToken);

        if (!result.IsSuccess)
        {
            Logger.LogWarning("Order {OrderId} not found", id);
            return NotFound(new { error = result.Error });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Create a new order
    /// </summary>
    /// <param name="command">Order creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created order</returns>
    /// <response code="201">Order created successfully</response>
    /// <response code="400">Invalid request data</response>
    [HttpPost]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOrder(
        [FromBody] CreateOrderCommand command,
        CancellationToken cancellationToken)
    {
        Logger.LogInformation("Creating order for table {TableId}", command.TableId);
        var result = await Mediator.Send(command, cancellationToken);
        return CreatedResult(result, nameof(GetOrderById), new { id = result.Data!.Id });
    }

    /// <summary>
    /// Update order status
    /// </summary>
    /// <param name="id">Order ID</param>
    /// <param name="request">Status update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content</returns>
    /// <response code="204">Status updated successfully</response>
    /// <response code="400">Invalid request data</response>
    /// <response code="404">Order not found</response>
    [HttpPatch("{id:guid}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateOrderStatus(
        Guid id,
        [FromBody] UpdateOrderStatusRequest request,
        CancellationToken cancellationToken)
    {
        Logger.LogInformation("Updating order {OrderId} status to {Status}", id, request.Status);

        var command = new UpdateOrderStatusCommand
        {
            OrderId = id,
            Status = request.Status,
            ActorId = request.ActorId
        };

        var result = await Mediator.Send(command, cancellationToken);
        return NoContentResult(result);
    }

    /// <summary>
    /// Upgrade buffet type for an order
    /// </summary>
    /// <param name="id">Order ID</param>
    /// <param name="request">Upgrade request data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content</returns>
    /// <response code="204">Buffet upgraded successfully</response>
    /// <response code="400">Invalid request data</response>
    /// <response code="404">Order not found</response>
    [HttpPost("{id:guid}/upgrade-buffet")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpgradeBuffetType(
        Guid id,
        [FromBody] UpgradeBuffetTypeRequest request,
        CancellationToken cancellationToken)
    {
        Logger.LogInformation("Upgrading buffet type for order {OrderId}", id);

        var command = new UpgradeBuffetTypeCommand
        {
            OrderId = id,
            ActorId = request.ActorId
        };

        var result = await Mediator.Send(command, cancellationToken);
        return NoContentResult(result);
    }
}

/// <summary>
/// Request to update order status
/// </summary>
/// <param name="Status">New status</param>
/// <param name="ActorId">ID of the user making the change</param>
public record UpdateOrderStatusRequest(string Status, Guid ActorId);

/// <summary>
/// Request to upgrade buffet type
/// </summary>
/// <param name="ActorId">ID of the user making the change</param>
public record UpgradeBuffetTypeRequest(Guid ActorId);

