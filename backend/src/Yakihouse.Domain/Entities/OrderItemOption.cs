using Yakihouse.Domain.Common;

namespace Yakihouse.Domain.Entities;

public class OrderItemOption : EntityBase
{
    private OrderItemOption()
    {
    }

    internal OrderItemOption(Guid orderItemId, Guid menuOptionId, string optionName, decimal extraPrice, int quantity)
    {
        OrderItemId = orderItemId;
        MenuOptionId = menuOptionId;
        OptionName = optionName;
        ExtraPrice = extraPrice;
        Quantity = quantity;
    }

    public Guid OrderItemId { get; private set; }
    public OrderItem OrderItem { get; private set; } = null!;
    public Guid MenuOptionId { get; private set; }
    public string OptionName { get; private set; } = null!;
    public decimal ExtraPrice { get; private set; }
    public int Quantity { get; private set; }

    public void UpdateQuantity(int quantity)
    {
        Quantity = quantity;
        Touch();
    }

    public void UpdateOptionInfo(string optionName, decimal extraPrice)
    {
        OptionName = optionName;
        ExtraPrice = extraPrice;
        Touch();
    }
}

