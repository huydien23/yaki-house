using Yakihouse.Application.Orders.DTOs;

namespace Yakihouse.Application.Common.Interfaces;

public interface IOrderQueryService
{
    Task<OrderDto?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<List<OrderDto>> GetActiveOrdersAsync(CancellationToken cancellationToken = default);
    Task<List<OrderDto>> GetOrdersByTableIdAsync(Guid tableId, CancellationToken cancellationToken = default);
}

