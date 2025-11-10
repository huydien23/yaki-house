using Yakihouse.Domain.Entities;

namespace Yakihouse.Application.Billing.Services;

public interface IBillCalculationService
{
    Task<BillCalculationResult> CalculateBillAsync(Order order, CancellationToken cancellationToken = default);
}

public record BillCalculationResult
{
    public decimal BuffetTicketTotal { get; init; }
    public decimal MenuItemsTotal { get; init; }
    public decimal SubTotal { get; init; }
    public decimal DiscountTotal { get; init; }
    public decimal ServiceCharge { get; init; }
    public decimal Tax { get; init; }
    public decimal GrandTotal { get; init; }
    public string CalculationDetails { get; init; } = string.Empty;
}

