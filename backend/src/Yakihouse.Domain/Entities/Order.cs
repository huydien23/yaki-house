using Yakihouse.Domain.Common;
using Yakihouse.Domain.Enums;

namespace Yakihouse.Domain.Entities;

public class Order : AggregateRoot
{
    private readonly List<OrderItem> _items = new();
    private readonly List<OrderAudit> _audits = new();

    private Order()
    {
    }

    public Order(Guid tableId, Guid staffId, int guestCount, string? notes)
    {
        TableId = tableId;
        StaffId = staffId;
        GuestCount = guestCount;
        Notes = notes;
        Status = OrderStatus.Draft;
        AdultCount = guestCount; // Default: tất cả là người lớn
        ChildCount = 0;
        BuffetType = "Nuong"; // Default: vé buffet nướng
        HasDessertBuffet = false;
    }

    public Guid TableId { get; private set; }
    public DiningTable Table { get; private set; } = null!;
    public Guid StaffId { get; private set; }
    public Staff Staff { get; private set; } = null!;
    public OrderStatus Status { get; private set; }
    public int GuestCount { get; private set; }
    public int AdultCount { get; private set; }
    public int ChildCount { get; private set; }
    public string? ChildHeights { get; private set; } // JSON array: [1.1, 1.2, 0.9] (mét)
    public string BuffetType { get; private set; } = "Nuong"; // "Nuong" hoặc "NuongLau"
    public bool HasDessertBuffet { get; private set; }
    public string? Notes { get; private set; }
    public DateTime? ClosedAt { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public IReadOnlyCollection<OrderAudit> Audits => _audits.AsReadOnly();

    public OrderItem AddItem(Guid menuItemId, string name, decimal unitPrice, int quantity, string? note)
    {
        var item = new OrderItem(Id, menuItemId, name, unitPrice, quantity, note);
        _items.Add(item);
        Touch();
        return item;
    }

    public void UpdateGuestCount(int guestCount)
    {
        GuestCount = guestCount;
        Touch();
    }

    public void UpdateBuffetInfo(int adultCount, int childCount, string? childHeights, string buffetType, bool hasDessertBuffet)
    {
        AdultCount = adultCount;
        ChildCount = childCount;
        ChildHeights = childHeights;
        BuffetType = buffetType;
        HasDessertBuffet = hasDessertBuffet;
        GuestCount = adultCount + childCount;
        Touch();
    }

    public void UpgradeBuffetType()
    {
        if (BuffetType == "Nuong")
        {
            BuffetType = "NuongLau";
            Touch();
        }
    }

    public void UpdateNotes(string? notes)
    {
        Notes = notes;
        Touch();
    }

    public void ChangeStatus(OrderStatus status)
    {
        Status = status;
        if (status == OrderStatus.Completed || status == OrderStatus.Cancelled)
        {
            ClosedAt = DateTime.UtcNow;
        }

        Touch();
    }

    public void RecordAudit(string actionType, Guid actorId, string? metadata)
    {
        var audit = new OrderAudit(Id, actionType, actorId, metadata);
        _audits.Add(audit);
        Touch();
    }
}

