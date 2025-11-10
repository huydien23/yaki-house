using Yakihouse.Domain.Common;
using Yakihouse.Domain.Enums;

namespace Yakihouse.Domain.Entities;

public class Payment : EntityBase
{
    private Payment()
    {
    }

    internal Payment(Guid billId, PaymentMethod method, decimal amount, string? referenceCode)
    {
        BillId = billId;
        Method = method;
        Amount = amount;
        ReferenceCode = referenceCode;
        Status = PaymentStatus.Pending;
    }

    public Guid BillId { get; private set; }
    public Bill Bill { get; private set; } = null!;
    public PaymentMethod Method { get; private set; }
    public decimal Amount { get; private set; }
    public string? ReferenceCode { get; private set; }
    public PaymentStatus Status { get; private set; }
    public DateTime? PaidAt { get; private set; }

    public void MarkConfirmed(DateTime paidAt)
    {
        Status = PaymentStatus.Confirmed;
        PaidAt = paidAt;
        Touch();
    }

    public void MarkFailed()
    {
        Status = PaymentStatus.Failed;
        Touch();
    }

    public void MarkRefunded()
    {
        Status = PaymentStatus.Refunded;
        Touch();
    }
}

