using Yakihouse.Domain.Common;
using Yakihouse.Domain.Enums;

namespace Yakihouse.Domain.Entities;

public class Bill : AggregateRoot
{
    private readonly List<Payment> _payments = new();
    private readonly List<BillPromotion> _promotions = new();

    private Bill()
    {
    }

    public Bill(Guid orderId)
    {
        OrderId = orderId;
        Status = BillStatus.Draft;
    }

    public Guid OrderId { get; private set; }
    public Order Order { get; private set; } = null!;
    public decimal SubTotal { get; private set; }
    public decimal DiscountTotal { get; private set; }
    public decimal ServiceCharge { get; private set; }
    public decimal Tax { get; private set; }
    public decimal GrandTotal { get; private set; }
    public BillStatus Status { get; private set; }
    public IReadOnlyCollection<Payment> Payments => _payments.AsReadOnly();
    public IReadOnlyCollection<BillPromotion> Promotions => _promotions.AsReadOnly();

    public void UpdateAmounts(decimal subTotal, decimal discountTotal, decimal serviceCharge, decimal tax)
    {
        SubTotal = subTotal;
        DiscountTotal = discountTotal;
        ServiceCharge = serviceCharge;
        Tax = tax;
        GrandTotal = subTotal - discountTotal + serviceCharge + tax;
        Touch();
    }

    public void ChangeStatus(BillStatus status)
    {
        Status = status;
        Touch();
    }

    public Payment AddPayment(PaymentMethod method, decimal amount, string? referenceCode)
    {
        var payment = new Payment(Id, method, amount, referenceCode);
        _payments.Add(payment);
        Touch();
        return payment;
    }

    public void ApplyPromotion(Guid promotionId, decimal appliedValue)
    {
        var promotion = new BillPromotion(Id, promotionId, appliedValue);
        _promotions.Add(promotion);
        Touch();
    }
}

