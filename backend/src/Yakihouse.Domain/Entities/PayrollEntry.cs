using Yakihouse.Domain.Common;

namespace Yakihouse.Domain.Entities;

public class PayrollEntry : AggregateRoot
{
    private PayrollEntry()
    {
    }

    public PayrollEntry(Guid staffId, string period)
    {
        StaffId = staffId;
        Period = period;
    }

    public Guid StaffId { get; private set; }
    public Staff Staff { get; private set; } = null!;
    public string Period { get; private set; } = null!;
    public decimal GrossPay { get; private set; }
    public decimal Adjustments { get; private set; }
    public decimal NetPay { get; private set; }
    public string Status { get; private set; } = "Draft";

    public void UpdateAmounts(decimal grossPay, decimal adjustments)
    {
        GrossPay = grossPay;
        Adjustments = adjustments;
        NetPay = grossPay + adjustments;
        Touch();
    }

    public void SetStatus(string status)
    {
        Status = status;
        Touch();
    }
}

