using Yakihouse.Domain.Common;

namespace Yakihouse.Domain.Entities;

public class PayrollSetting : AggregateRoot
{
    private PayrollSetting()
    {
    }

    public PayrollSetting(Guid roleId, decimal baseRate, decimal allowance)
    {
        RoleId = roleId;
        BaseRate = baseRate;
        Allowance = allowance;
    }

    public Guid RoleId { get; private set; }
    public Role Role { get; private set; } = null!;
    public decimal BaseRate { get; private set; }
    public decimal Allowance { get; private set; }

    public void UpdateRates(decimal baseRate, decimal allowance)
    {
        BaseRate = baseRate;
        Allowance = allowance;
        Touch();
    }
}

