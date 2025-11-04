using Yakihouse.Domain.Common;

namespace Yakihouse.Domain.Entities;

public class InventoryItem : AggregateRoot
{
    private readonly List<InventoryTransaction> _transactions = new();

    private InventoryItem()
    {
    }

    public InventoryItem(string name, string unit, decimal safetyStock)
    {
        Name = name;
        Unit = unit;
        SafetyStock = safetyStock;
    }

    public string Name { get; private set; } = null!;
    public string Unit { get; private set; } = null!;
    public decimal SafetyStock { get; private set; }
    public decimal CurrentStock { get; private set; }
    public IReadOnlyCollection<InventoryTransaction> Transactions => _transactions.AsReadOnly();

    public void UpdateInfo(string name, string unit, decimal safetyStock)
    {
        Name = name;
        Unit = unit;
        SafetyStock = safetyStock;
        Touch();
    }

    public void AdjustStock(decimal quantity)
    {
        CurrentStock += quantity;
        Touch();
    }

    public InventoryTransaction AddTransaction(InventoryTransaction transaction)
    {
        _transactions.Add(transaction);
        AdjustStock(transaction.Quantity);
        return transaction;
    }
}

