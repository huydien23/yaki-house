using Yakihouse.Application.Orders.DTOs;

namespace Yakihouse.Application.Common.Interfaces;

public interface IOrderNotificationService
{
    Task NotifyOrderCreatedAsync(OrderDto order);
    Task NotifyOrderUpdatedAsync(OrderDto order);
    Task NotifyOrderStatusChangedAsync(Guid orderId, string status);
}
