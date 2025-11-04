using Yakihouse.Domain.Common;
using Yakihouse.Domain.Enums;

namespace Yakihouse.Domain.Entities;

public class KitchenTicket : AggregateRoot
{
    private readonly List<KitchenTicketItem> _items = new();

    private KitchenTicket()
    {
    }

    public KitchenTicket(Guid orderId, Guid stationId)
    {
        OrderId = orderId;
        StationId = stationId;
        Status = KitchenTicketStatus.Pending;
    }

    public Guid OrderId { get; private set; }
    public Order Order { get; private set; } = null!;
    public Guid StationId { get; private set; }
    public KitchenStation Station { get; private set; } = null!;
    public KitchenTicketStatus Status { get; private set; }
    public DateTime? StartedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public IReadOnlyCollection<KitchenTicketItem> Items => _items.AsReadOnly();

    public KitchenTicketItem AddItem(Guid orderItemId, int quantity, string? notes)
    {
        var item = new KitchenTicketItem(Id, orderItemId, quantity, notes);
        _items.Add(item);
        Touch();
        return item;
    }

    public void Start()
    {
        if (Status == KitchenTicketStatus.Pending)
        {
            Status = KitchenTicketStatus.InProgress;
            StartedAt = DateTime.UtcNow;
            Touch();
        }
    }

    public void Complete()
    {
        Status = KitchenTicketStatus.Completed;
        CompletedAt = DateTime.UtcNow;
        Touch();
    }

    public void MarkReady()
    {
        Status = KitchenTicketStatus.Ready;
        Touch();
    }

    public void Cancel()
    {
        Status = KitchenTicketStatus.Cancelled;
        CompletedAt = DateTime.UtcNow;
        Touch();
    }
}

