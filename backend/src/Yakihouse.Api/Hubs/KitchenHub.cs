using Microsoft.AspNetCore.SignalR;

namespace Yakihouse.Api.Hubs;

public class KitchenHub : Hub
{
    public async Task JoinKitchenGroup()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "kitchen");
    }

    public async Task LeaveKitchenGroup()
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "kitchen");
    }

    public async Task JoinStationGroup(string stationId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"station_{stationId}");
    }

    public async Task NotifyTicketStatusChanged(string ticketId, string status)
    {
        await Clients.All.SendAsync("TicketStatusChanged", new { ticketId, status });
    }

    public async Task NotifyItemStatusChanged(string itemId, string status)
    {
        await Clients.All.SendAsync("ItemStatusChanged", new { itemId, status });
    }
}

