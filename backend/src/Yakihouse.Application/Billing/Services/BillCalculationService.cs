using System.Text.Json;
using Yakihouse.Domain.Entities;
using Yakihouse.Domain.Repositories;

namespace Yakihouse.Application.Billing.Services;

public class BillCalculationService : IBillCalculationService
{
    private readonly IRepository<MenuItem> _menuItemRepository;
    private const decimal BUFFET_NUONG_PRICE = 175000m;
    private const decimal BUFFET_NUONG_LAU_PRICE = 199000m;
    private const decimal BUFFET_TRE_EM_PRICE = 79000m;
    private const decimal BUFFET_TRANG_MIENG_PRICE = 45000m;

    public BillCalculationService(IRepository<MenuItem> menuItemRepository)
    {
        _menuItemRepository = menuItemRepository;
    }

    public async Task<BillCalculationResult> CalculateBillAsync(Order order, CancellationToken cancellationToken = default)
    {
        var details = new List<string>();
        decimal buffetTicketTotal = 0m;
        decimal menuItemsTotal = 0m;

        // 1. Tính vé buffet
        buffetTicketTotal = CalculateBuffetTickets(order, details);

        // 2. Tính món ăn (đồ uống gọi thêm, món tính phí riêng)
        menuItemsTotal = await CalculateMenuItemsAsync(order, cancellationToken, details);

        // 3. Tính tổng
        decimal subTotal = buffetTicketTotal + menuItemsTotal;
        decimal discountTotal = 0m; // Sẽ được tính bởi PromotionService
        decimal serviceCharge = 0m; // Có thể cấu hình sau
        decimal tax = 0m; // Có thể cấu hình sau
        decimal grandTotal = subTotal - discountTotal + serviceCharge + tax;

        return new BillCalculationResult
        {
            BuffetTicketTotal = buffetTicketTotal,
            MenuItemsTotal = menuItemsTotal,
            SubTotal = subTotal,
            DiscountTotal = discountTotal,
            ServiceCharge = serviceCharge,
            Tax = tax,
            GrandTotal = grandTotal,
            CalculationDetails = string.Join("\n", details)
        };
    }

    private decimal CalculateBuffetTickets(Order order, List<string> details)
    {
        decimal total = 0m;

        // Tính vé người lớn
        decimal adultTicketPrice = order.BuffetType == "NuongLau" 
            ? BUFFET_NUONG_LAU_PRICE 
            : BUFFET_NUONG_PRICE;

        decimal adultTotal = order.AdultCount * adultTicketPrice;
        total += adultTotal;
        details.Add($"Vé buffet người lớn ({order.AdultCount} × {adultTicketPrice:N0}): {adultTotal:N0} VNĐ");

        // Tính vé trẻ em theo chiều cao
        if (order.ChildCount > 0 && !string.IsNullOrEmpty(order.ChildHeights))
        {
            var heights = JsonSerializer.Deserialize<List<decimal>>(order.ChildHeights) ?? new List<decimal>();
            int freeCount = 0;
            int childTicketCount = 0;
            int adultTicketChildCount = 0;

            foreach (var height in heights)
            {
                if (height < 1.0m)
                {
                    freeCount++;
                }
                else if (height >= 1.0m && height <= 1.3m)
                {
                    childTicketCount++;
                }
                else
                {
                    adultTicketChildCount++;
                }
            }

            if (freeCount > 0)
            {
                details.Add($"Trẻ em dưới 1m ({freeCount}): Miễn phí vé buffet chính");
            }

            if (childTicketCount > 0)
            {
                decimal childTotal = childTicketCount * BUFFET_TRE_EM_PRICE;
                total += childTotal;
                details.Add($"Vé buffet trẻ em ({childTicketCount} × {BUFFET_TRE_EM_PRICE:N0}): {childTotal:N0} VNĐ");
            }

            if (adultTicketChildCount > 0)
            {
                decimal adultChildTotal = adultTicketChildCount * adultTicketPrice;
                total += adultChildTotal;
                details.Add($"Trẻ em trên 1.3m ({adultTicketChildCount} × {adultTicketPrice:N0}): {adultChildTotal:N0} VNĐ");
            }

            // Vé tráng miệng cho trẻ em dưới 1m (nếu có)
            if (order.HasDessertBuffet && freeCount > 0)
            {
                decimal dessertForFreeKids = freeCount * BUFFET_TRANG_MIENG_PRICE;
                total += dessertForFreeKids;
                details.Add($"Vé tráng miệng cho trẻ dưới 1m ({freeCount} × {BUFFET_TRANG_MIENG_PRICE:N0}): {dessertForFreeKids:N0} VNĐ");
            }
        }

        // Vé tráng miệng (nếu có)
        if (order.HasDessertBuffet)
        {
            int dessertCount = order.GuestCount; // Tất cả khách
            decimal dessertTotal = dessertCount * BUFFET_TRANG_MIENG_PRICE;
            total += dessertTotal;
            details.Add($"Vé buffet tráng miệng ({dessertCount} × {BUFFET_TRANG_MIENG_PRICE:N0}): {dessertTotal:N0} VNĐ");
        }

        return total;
    }

    private async Task<decimal> CalculateMenuItemsAsync(Order order, CancellationToken cancellationToken, List<string> details)
    {
        decimal total = 0m;

        // Lấy category "Vé Buffet" để loại trừ
        var buffetCategoryName = "Vé Buffet";

        foreach (var item in order.Items)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(item.MenuItemId, cancellationToken);
            if (menuItem == null) continue;

            // Bỏ qua vé buffet (đã tính riêng)
            if (menuItem.Category.Name == buffetCategoryName)
                continue;

            // Tính món ăn (đồ uống gọi thêm, món tính phí riêng)
            decimal itemTotal = item.UnitPrice * item.Quantity;

            // Tính options
            foreach (var option in item.Options)
            {
                itemTotal += option.ExtraPrice * option.Quantity;
            }

            total += itemTotal;
            details.Add($"{item.MenuItemName} ({item.Quantity}): {itemTotal:N0} VNĐ");
        }

        return total;
    }
}

