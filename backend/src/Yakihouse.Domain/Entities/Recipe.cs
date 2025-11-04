using Yakihouse.Domain.Common;

namespace Yakihouse.Domain.Entities;

public class Recipe : EntityBase
{
    private Recipe()
    {
    }

    public Recipe(Guid menuItemId, Guid inventoryItemId, decimal quantityPerServing)
    {
        MenuItemId = menuItemId;
        InventoryItemId = inventoryItemId;
        QuantityPerServing = quantityPerServing;
    }

    public Guid MenuItemId { get; private set; }
    public MenuItem MenuItem { get; private set; } = null!;
    public Guid InventoryItemId { get; private set; }
    public InventoryItem InventoryItem { get; private set; } = null!;
    public decimal QuantityPerServing { get; private set; }

    public void UpdateQuantity(decimal quantityPerServing)
    {
        QuantityPerServing = quantityPerServing;
        Touch();
    }
}

