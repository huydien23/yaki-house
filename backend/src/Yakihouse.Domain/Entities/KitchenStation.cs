using Yakihouse.Domain.Common;

namespace Yakihouse.Domain.Entities;

public class KitchenStation : AggregateRoot
{
    private readonly List<KitchenTicket> _tickets = new();

    private KitchenStation()
    {
    }

    public KitchenStation(string name, int displayOrder)
    {
        Name = name;
        DisplayOrder = displayOrder;
        IsActive = true;
    }

    public string Name { get; private set; } = null!;
    public int DisplayOrder { get; private set; }
    public bool IsActive { get; private set; }
    public IReadOnlyCollection<KitchenTicket> Tickets => _tickets.AsReadOnly();

    public void UpdateDetails(string name, int displayOrder)
    {
        Name = name;
        DisplayOrder = displayOrder;
        Touch();
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
        Touch();
    }
}

