using Yakihouse.Domain.Common;

namespace Yakihouse.Domain.Entities;

public class BillPromotion : EntityBase
{
    private BillPromotion()
    {
    }

    internal BillPromotion(Guid billId, Guid promotionId, decimal appliedValue)
    {
        BillId = billId;
        PromotionId = promotionId;
        AppliedValue = appliedValue;
    }

    public Guid BillId { get; private set; }
    public Bill Bill { get; private set; } = null!;
    public Guid PromotionId { get; private set; }
    public Promotion Promotion { get; private set; } = null!;
    public decimal AppliedValue { get; private set; }
}

