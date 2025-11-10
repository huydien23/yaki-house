using Microsoft.AspNetCore.SignalR;

namespace Yakihouse.Api.Hubs;

public class OrderHub : Hub
{
    public async Task JoinTableGroup(string tableId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"table_{tableId}");
    }

    public async Task LeaveTableGroup(string tableId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"table_{tableId}");
    }

    public async Task JoinStaffGroup(string staffId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"staff_{staffId}");
    }

    public async Task NotifyOrderCreated(string tableId, object orderData)
    {
        await Clients.Group($"table_{tableId}").SendAsync("OrderCreated", orderData);
        await Clients.Group("kitchen").SendAsync("NewOrder", orderData);
    }

    public async Task NotifyOrderUpdated(string orderId, object orderData)
    {
        await Clients.All.SendAsync("OrderUpdated", orderData);
    }

    public async Task NotifyOrderStatusChanged(string orderId, string status)
    {
        await Clients.All.SendAsync("OrderStatusChanged", new { orderId, status });
    }
}

