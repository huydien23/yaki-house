using MediatR;
using Yakihouse.Application.Common.Interfaces;
using Yakihouse.Application.Common.Models;
using Yakihouse.Application.Orders.DTOs;

namespace Yakihouse.Application.Orders.Queries.GetActiveOrders;

public class GetActiveOrdersQueryHandler : IRequestHandler<GetActiveOrdersQuery, Result<List<OrderDto>>>
{
    private readonly IOrderQueryService _orderQueryService;

    public GetActiveOrdersQueryHandler(IOrderQueryService orderQueryService)
    {
        _orderQueryService = orderQueryService;
    }

    public async Task<Result<List<OrderDto>>> Handle(GetActiveOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderQueryService.GetActiveOrdersAsync(cancellationToken);
        return Result<List<OrderDto>>.Success(orders);
    }
}

