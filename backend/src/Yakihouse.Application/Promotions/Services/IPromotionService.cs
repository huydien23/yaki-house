using Yakihouse.Domain.Entities;

namespace Yakihouse.Application.Promotions.Services;

public interface IPromotionService
{
    Task<List<ApplicablePromotion>> GetApplicablePromotionsAsync(Order order, CancellationToken cancellationToken = default);
    Task<decimal> CalculateDiscountAsync(Order order, Guid promotionId, CancellationToken cancellationToken = default);
}

public record ApplicablePromotion
{
    public Guid PromotionId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public decimal DiscountValue { get; init; }
    public bool IsApplicable { get; init; }
    public string Reason { get; init; } = string.Empty;
}

