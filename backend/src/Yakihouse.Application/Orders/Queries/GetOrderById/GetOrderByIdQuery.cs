using MediatR;
using Yakihouse.Application.Common.Models;
using Yakihouse.Application.Orders.DTOs;

namespace Yakihouse.Application.Orders.Queries.GetOrderById;

public record GetOrderByIdQuery(Guid OrderId) : IRequest<Result<OrderDto>>;

