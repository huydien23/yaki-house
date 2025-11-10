using Microsoft.AspNetCore.SignalR;
using Yakihouse.Api.Hubs;
using Yakihouse.Application.Common.Interfaces;
using Yakihouse.Application.Orders.DTOs;

namespace Yakihouse.Api.Services;

public class OrderNotificationService : IOrderNotificationService
{
    private readonly IHubContext<OrderHub> _orderHubContext;
    private readonly IHubContext<KitchenHub> _kitchenHubContext;

    public OrderNotificationService(
        IHubContext<OrderHub> orderHubContext,
        IHubContext<KitchenHub> kitchenHubContext)
    {
        _orderHubContext = orderHubContext;
        _kitchenHubContext = kitchenHubContext;
    }

    public async Task NotifyOrderCreatedAsync(OrderDto order)
    {
        await _orderHubContext.Clients.All.SendAsync("OrderCreated", new
        {
            order.Id,
            order.TableId,
            order.TableCode,
            order.Status,
            order.CreatedAt,
            ItemCount = order.Items?.Count ?? 0
        });

        await _kitchenHubContext.Clients.All.SendAsync("NewOrderReceived", new
        {
            order.Id,
            order.TableCode,
            Items = order.Items?.Select(i => new
            {
                i.MenuItemId,
                i.MenuItemName,
                i.Quantity,
                i.Note
            })
        });
    }

    public async Task NotifyOrderStatusChangedAsync(Guid orderId, string status)
    {
        await _orderHubContext.Clients.All.SendAsync("OrderStatusChanged", new
        {
            OrderId = orderId,
            Status = status,
            ChangedAt = DateTime.UtcNow
        });

        // Notify kitchen
        await _kitchenHubContext.Clients.All.SendAsync("OrderStatusUpdated", new
        {
            OrderId = orderId,
            Status = status
        });
    }

    public async Task NotifyOrderUpdatedAsync(OrderDto order)
    {
        await _orderHubContext.Clients.All.SendAsync("OrderUpdated", new
        {
            order.Id,
            order.Status,
            UpdatedAt = DateTime.UtcNow,
            ItemCount = order.Items?.Count ?? 0
        });
    }
}
