using Microsoft.EntityFrameworkCore;
using Yakihouse.Application.Kitchen.Services;
using Yakihouse.Domain.Entities;
using Yakihouse.Domain.Repositories;
using Yakihouse.Infrastructure.Persistence;

namespace Yakihouse.Infrastructure.Services;

public class KitchenTicketService : IKitchenTicketService
{
    private readonly YakihouseDbContext _context;
    private readonly IRepository<KitchenStation> _kitchenStationRepository;
    private readonly IRepository<MenuItem> _menuItemRepository;

    public KitchenTicketService(
        YakihouseDbContext context,
        IRepository<KitchenStation> kitchenStationRepository,
        IRepository<MenuItem> menuItemRepository)
    {
        _context = context;
        _kitchenStationRepository = kitchenStationRepository;
        _menuItemRepository = menuItemRepository;
    }

    public async Task<List<KitchenTicket>> CreateTicketsForOrderAsync(Order order, CancellationToken cancellationToken = default)
    {
        var tickets = new List<KitchenTicket>();

        // Nhóm order items theo KitchenStation
        var itemsByStation = new Dictionary<Guid, List<OrderItem>>();

        foreach (var orderItem in order.Items)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(orderItem.MenuItemId, cancellationToken);
            if (menuItem == null || !menuItem.KitchenStationId.HasValue)
                continue; // Bỏ qua món không thuộc bếp nào (ví dụ: vé buffet)

            var stationId = menuItem.KitchenStationId.Value;

            if (!itemsByStation.ContainsKey(stationId))
            {
                itemsByStation[stationId] = new List<OrderItem>();
            }

            itemsByStation[stationId].Add(orderItem);
        }

        // Tạo KitchenTicket cho mỗi bếp
        foreach (var kvp in itemsByStation)
        {
            var stationId = kvp.Key;
            var items = kvp.Value;

            var station = await _kitchenStationRepository.GetByIdAsync(stationId, cancellationToken);
            if (station == null || !station.IsActive)
                continue;

            var ticket = new KitchenTicket(order.Id, stationId);
            
            foreach (var item in items)
            {
                var ticketItem = ticket.AddItem(item.Id, item.Quantity, item.Note);
            }

            tickets.Add(ticket);
            await _context.KitchenTickets.AddAsync(ticket, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return tickets;
    }
}

