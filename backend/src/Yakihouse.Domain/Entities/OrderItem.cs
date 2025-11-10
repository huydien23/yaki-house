using Yakihouse.Domain.Common;
using Yakihouse.Domain.Enums;

namespace Yakihouse.Domain.Entities;

public class OrderItem : EntityBase
{
    private readonly List<OrderItemOption> _options = new();

    private OrderItem()
    {
    }

    internal OrderItem(Guid orderId, Guid menuItemId, string menuItemName, decimal unitPrice, int quantity, string? note)
    {
        OrderId = orderId;
        MenuItemId = menuItemId;
        MenuItemName = menuItemName;
        UnitPrice = unitPrice;
        Quantity = quantity;
        Note = note;
        Status = OrderItemStatus.Pending;
    }

    public Guid OrderId { get; private set; }
    public Order Order { get; private set; } = null!;
    public Guid MenuItemId { get; private set; }
    public string MenuItemName { get; private set; } = null!;
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public string? Note { get; private set; }
    public OrderItemStatus Status { get; private set; }
    public IReadOnlyCollection<OrderItemOption> Options => _options.AsReadOnly();

    public void UpdateQuantity(int quantity)
    {
        Quantity = quantity;
        Touch();
    }

    public void UpdateNote(string? note)
    {
        Note = note;
        Touch();
    }

    public void ChangeStatus(OrderItemStatus status)
    {
        Status = status;
        Touch();
    }

    public OrderItemOption AddOption(Guid menuOptionId, string optionName, decimal extraPrice, int quantity)
    {
        var option = new OrderItemOption(Id, menuOptionId, optionName, extraPrice, quantity);
        _options.Add(option);
        Touch();
        return option;
    }
}

