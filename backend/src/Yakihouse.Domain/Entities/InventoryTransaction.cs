using Yakihouse.Domain.Common;
using Yakihouse.Domain.Enums;

namespace Yakihouse.Domain.Entities;

public class InventoryTransaction : EntityBase
{
    private InventoryTransaction()
    {
    }

    public InventoryTransaction(Guid inventoryItemId, InventoryTransactionType type, decimal quantity, Guid? referenceId)
    {
        InventoryItemId = inventoryItemId;
        Type = type;
        Quantity = quantity;
        ReferenceId = referenceId;
    }

    public Guid InventoryItemId { get; private set; }
    public InventoryItem InventoryItem { get; private set; } = null!;
    public InventoryTransactionType Type { get; private set; }
    public decimal Quantity { get; private set; }
    public Guid? ReferenceId { get; private set; }
}

