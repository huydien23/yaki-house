using Yakihouse.Domain.Common;
using Yakihouse.Domain.Enums;

namespace Yakihouse.Domain.Entities;

public class KitchenTicketItem : EntityBase
{
    private KitchenTicketItem()
    {
    }

    internal KitchenTicketItem(Guid ticketId, Guid orderItemId, int quantity, string? notes)
    {
        TicketId = ticketId;
        OrderItemId = orderItemId;
        Quantity = quantity;
        Notes = notes;
        Status = KitchenTicketItemStatus.Pending;
    }

    public Guid TicketId { get; private set; }
    public KitchenTicket Ticket { get; private set; } = null!;
    public Guid OrderItemId { get; private set; }
    public OrderItem OrderItem { get; private set; } = null!;
    public int Quantity { get; private set; }
    public string? Notes { get; private set; }
    public KitchenTicketItemStatus Status { get; private set; }

    public void UpdateNotes(string? notes)
    {
        Notes = notes;
        Touch();
    }

    public void ChangeStatus(KitchenTicketItemStatus status)
    {
        Status = status;
        Touch();
    }
}

