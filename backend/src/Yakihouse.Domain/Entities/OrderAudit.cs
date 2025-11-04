using Yakihouse.Domain.Common;

namespace Yakihouse.Domain.Entities;

public class OrderAudit : EntityBase
{
    private OrderAudit()
    {
    }

    internal OrderAudit(Guid orderId, string actionType, Guid actorId, string? metadata)
    {
        OrderId = orderId;
        ActionType = actionType;
        ActorId = actorId;
        Metadata = metadata;
    }

    public Guid OrderId { get; private set; }
    public Order Order { get; private set; } = null!;
    public string ActionType { get; private set; } = null!;
    public Guid ActorId { get; private set; }
    public string? Metadata { get; private set; }
}

