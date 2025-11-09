using Yakihouse.Domain.Entities;
using Yakihouse.Domain.Repositories;

namespace Yakihouse.Application.Promotions.Services;

public class PromotionService : IPromotionService
{
    private readonly IRepository<Promotion> _promotionRepository;

    public PromotionService(IRepository<Promotion> promotionRepository)
    {
        _promotionRepository = promotionRepository;
    }

    public async Task<List<ApplicablePromotion>> GetApplicablePromotionsAsync(Order order, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var dayOfWeek = now.DayOfWeek;
        var hour = now.Hour;

        var allPromotions = await _promotionRepository.GetAllAsync(cancellationToken);
        var promotions = allPromotions
            .Where(p => p.IsActive && p.StartDate <= now && p.EndDate >= now)
            .ToList();

        var applicablePromotions = new List<ApplicablePromotion>();

        foreach (var promotion in promotions)
        {
            var applicable = IsPromotionApplicable(promotion, order, dayOfWeek, hour, out var reason);
            var discountValue = applicable 
                ? await CalculateDiscountAsync(order, promotion.Id, cancellationToken)
                : 0m;

            applicablePromotions.Add(new ApplicablePromotion
            {
                PromotionId = promotion.Id,
                Name = promotion.Name,
                Type = promotion.Type,
                DiscountValue = discountValue,
                IsApplicable = applicable,
                Reason = reason
            });
        }

        return applicablePromotions;
    }

    public async Task<decimal> CalculateDiscountAsync(Order order, Guid promotionId, CancellationToken cancellationToken = default)
    {
        var promotion = await _promotionRepository.GetByIdAsync(promotionId, cancellationToken);
        if (promotion == null || !promotion.IsActive)
            return 0m;

        var now = DateTime.UtcNow;
        var dayOfWeek = now.DayOfWeek;
        var hour = now.Hour;

        if (!IsPromotionApplicable(promotion, order, dayOfWeek, hour, out _))
            return 0m;

        return promotion.Type switch
        {
            "GroupDiscount" => CalculateGroupDiscount(promotion, order),
            "FixedDiscount" => promotion.Value,
            "PerPersonDiscount" => promotion.Value * order.GuestCount,
            _ => 0m
        };
    }

    private bool IsPromotionApplicable(Promotion promotion, Order order, DayOfWeek dayOfWeek, int hour, out string reason)
    {
        reason = string.Empty;

        // Kiểm tra ngày trong tuần (không áp dụng Thứ 7, Chủ Nhật)
        if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
        {
            if (promotion.Conditions?.Contains("Thứ 2 - Thứ 6") == true || 
                promotion.Conditions?.Contains("không áp dụng cuối tuần") == true)
            {
                reason = "Không áp dụng vào cuối tuần";
                return false;
            }
        }

        // Kiểm tra giờ vàng (11:00 - 17:00)
        if (promotion.Name.Contains("Giờ Vàng"))
        {
            if (hour < 11 || hour >= 17)
            {
                reason = "Chỉ áp dụng từ 11:00 - 17:00";
                return false;
            }
        }

        // Kiểm tra số khách (Đặt bàn trước: từ 4 khách)
        if (promotion.Name.Contains("Đặt Bàn Trước"))
        {
            if (order.GuestCount < 4)
            {
                reason = "Cần từ 4 khách trở lên";
                return false;
            }
        }

        // Kiểm tra ngày lễ (cần implement sau)
        // TODO: Kiểm tra danh sách ngày lễ

        return true;
    }

    private decimal CalculateGroupDiscount(Promotion promotion, Order order)
    {
        if (promotion.Name.Contains("Đi 4 Tính 3"))
        {
            // Lũy tiến: 4 tính 3, 8 tính 6, 12 tính 9...
            int groups = order.GuestCount / 4;
            int freeGuests = groups; // Mỗi nhóm 4 được miễn phí 1
            int payingGuests = order.GuestCount - freeGuests;

            // Tính giá vé trung bình
            decimal avgTicketPrice = order.BuffetType == "NuongLau" ? 199000m : 175000m;
            decimal discount = freeGuests * avgTicketPrice;

            return discount;
        }
        else if (promotion.Name.Contains("Đi 4 Giảm 100k"))
        {
            // Lũy tiến: 4 giảm 100k, 8 giảm 200k, 12 giảm 300k...
            int groups = order.GuestCount / 4;
            decimal discount = groups * promotion.Value;

            return discount;
        }

        return 0m;
    }
}

