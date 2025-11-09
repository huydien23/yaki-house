namespace Yakihouse.Application.Billing.DTOs;

public record BillDto
{
    public Guid Id { get; init; }
    public Guid OrderId { get; init; }
    public decimal BuffetTicketTotal { get; init; }
    public decimal MenuItemsTotal { get; init; }
    public decimal SubTotal { get; init; }
    public decimal DiscountTotal { get; init; }
    public decimal ServiceCharge { get; init; }
    public decimal Tax { get; init; }
    public decimal GrandTotal { get; init; }
    public string Status { get; init; } = string.Empty;
    public string CalculationDetails { get; init; } = string.Empty;
    public List<AppliedPromotionDto> AppliedPromotions { get; init; } = new();
    public DateTime CreatedAt { get; init; }
}

public record AppliedPromotionDto
{
    public Guid PromotionId { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal DiscountValue { get; init; }
}

