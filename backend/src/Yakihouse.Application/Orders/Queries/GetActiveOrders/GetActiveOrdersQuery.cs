using MediatR;
using Yakihouse.Application.Common.Models;
using Yakihouse.Application.Orders.DTOs;

namespace Yakihouse.Application.Orders.Queries.GetActiveOrders;

public record GetActiveOrdersQuery : IRequest<Result<List<OrderDto>>>;

