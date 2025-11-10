using MediatR;
using Yakihouse.Application.Common.Models;

namespace Yakihouse.Application.Orders.Commands.UpdateOrderStatus;

public record UpdateOrderStatusCommand : IRequest<Result>
{
    public Guid OrderId { get; init; }
    public string Status { get; init; } = string.Empty;
    public Guid ActorId { get; init; }
}

