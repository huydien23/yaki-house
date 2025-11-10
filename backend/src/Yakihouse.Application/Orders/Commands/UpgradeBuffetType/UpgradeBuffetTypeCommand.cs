using MediatR;
using Yakihouse.Application.Common.Models;

namespace Yakihouse.Application.Orders.Commands.UpgradeBuffetType;

public record UpgradeBuffetTypeCommand : IRequest<Result>
{
    public Guid OrderId { get; init; }
    public Guid ActorId { get; init; }
}

