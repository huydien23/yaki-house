using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yakihouse.Domain.Enums;
using Yakihouse.Infrastructure.Persistence;

namespace Yakihouse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly YakihouseDbContext _context;

    public ReportsController(YakihouseDbContext context)
    {
        _context = context;
    }

    [HttpGet("overview")]
    public async Task<IActionResult> GetOverview([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, CancellationToken cancellationToken)
    {
        var from = fromDate ?? DateTime.Today.AddDays(-30);
        var to = toDate ?? DateTime.Today.AddDays(1);

        // Revenue from bills
        var revenue = await _context.Bills
            .Where(b => b.CreatedAt >= from && b.CreatedAt < to)
            .SumAsync(b => b.GrandTotal, cancellationToken);

        // Order count
        var orderCount = await _context.Orders
            .Where(o => o.CreatedAt >= from && o.CreatedAt < to)
            .CountAsync(cancellationToken);

        // Average order value
        var avgOrderValue = orderCount > 0 ? revenue / orderCount : 0;

        // Completed orders
        var completedOrders = await _context.Orders
            .Where(o => o.CreatedAt >= from && o.CreatedAt < to && o.Status == OrderStatus.Completed)
            .CountAsync(cancellationToken);

        // Customer count (unique tables served)
        var customerCount = await _context.Orders
            .Where(o => o.CreatedAt >= from && o.CreatedAt < to)
            .Select(o => o.TableId)
            .Distinct()
            .CountAsync(cancellationToken);

        return Ok(new
        {
            revenue,
            orderCount,
            avgOrderValue,
            completedOrders,
            customerCount,
            fromDate = from,
            toDate = to
        });
    }

    [HttpGet("revenue-by-date")]
    public async Task<IActionResult> GetRevenueByDate([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, CancellationToken cancellationToken)
    {
        var from = fromDate ?? DateTime.Today.AddDays(-30);
        var to = toDate ?? DateTime.Today.AddDays(1);

        var revenueByDate = await _context.Bills
            .Where(b => b.CreatedAt >= from && b.CreatedAt < to)
            .GroupBy(b => b.CreatedAt.Date)
            .Select(g => new
            {
                date = g.Key,
                revenue = g.Sum(b => b.GrandTotal),
                orderCount = g.Count()
            })
            .OrderBy(x => x.date)
            .ToListAsync(cancellationToken);

        return Ok(revenueByDate);
    }

    [HttpGet("top-items")]
    public async Task<IActionResult> GetTopItems([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, [FromQuery] int limit, CancellationToken cancellationToken)
    {
        var from = fromDate ?? DateTime.Today.AddDays(-30);
        var to = toDate ?? DateTime.Today.AddDays(1);

        var topItems = await _context.OrderItems
            .Include(oi => oi.Order)
            .Where(oi => oi.Order.CreatedAt >= from && oi.Order.CreatedAt < to)
            .GroupBy(oi => new { oi.MenuItemId, oi.MenuItemName })
            .Select(g => new
            {
                menuItemId = g.Key.MenuItemId,
                menuItemName = g.Key.MenuItemName,
                totalQuantity = g.Sum(oi => oi.Quantity),
                totalRevenue = g.Sum(oi => oi.UnitPrice * oi.Quantity),
                orderCount = g.Select(oi => oi.OrderId).Distinct().Count()
            })
            .OrderByDescending(x => x.totalQuantity)
            .Take(limit)
            .ToListAsync(cancellationToken);

        return Ok(topItems);
    }

    [HttpGet("staff-performance")]
    public async Task<IActionResult> GetStaffPerformance([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, CancellationToken cancellationToken)
    {
        var from = fromDate ?? DateTime.Today.AddDays(-30);
        var to = toDate ?? DateTime.Today.AddDays(1);

        var staffPerformance = await _context.Orders
            .Include(o => o.Staff)
            .Include(o => o.Items)
            .Where(o => o.CreatedAt >= from && o.CreatedAt < to && o.StaffId != null)
            .GroupBy(o => new { o.StaffId, o.Staff.FullName })
            .Select(g => new
            {
                staffId = g.Key.StaffId,
                staffName = g.Key.FullName,
                orderCount = g.Count(),
                completedOrders = g.Count(o => o.Status == OrderStatus.Completed),
                totalRevenue = g.SelectMany(o => o.Items).Sum(oi => oi.UnitPrice * oi.Quantity)
            })
            .OrderByDescending(x => x.orderCount)
            .ToListAsync(cancellationToken);

        return Ok(staffPerformance);
    }

    [HttpGet("category-sales")]
    public async Task<IActionResult> GetCategorySales([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, CancellationToken cancellationToken)
    {
        var from = fromDate ?? DateTime.Today.AddDays(-30);
        var to = toDate ?? DateTime.Today.AddDays(1);

        var categorySales = await _context.MenuItems
            .Include(mi => mi.Category)
            .GroupJoin(
                _context.OrderItems
                    .Include(oi => oi.Order)
                    .Where(oi => oi.Order.CreatedAt >= from && oi.Order.CreatedAt < to),
                mi => mi.Id,
                oi => oi.MenuItemId,
                (mi, orderItems) => new { MenuItem = mi, OrderItems = orderItems })
            .GroupBy(x => new { x.MenuItem.CategoryId, x.MenuItem.Category.Name })
            .Select(g => new
            {
                categoryId = g.Key.CategoryId,
                categoryName = g.Key.Name,
                totalQuantity = g.SelectMany(x => x.OrderItems).Sum(oi => oi.Quantity),
                totalRevenue = g.SelectMany(x => x.OrderItems).Sum(oi => oi.UnitPrice * oi.Quantity),
                itemCount = g.Select(x => x.MenuItem.Id).Distinct().Count()
            })
            .OrderByDescending(x => x.totalRevenue)
            .ToListAsync(cancellationToken);

        return Ok(categorySales);
    }

    [HttpGet("payment-methods")]
    public async Task<IActionResult> GetPaymentMethods([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, CancellationToken cancellationToken)
    {
        var from = fromDate ?? DateTime.Today.AddDays(-30);
        var to = toDate ?? DateTime.Today.AddDays(1);

        var paymentMethods = await _context.Payments
            .Where(p => p.CreatedAt >= from && p.CreatedAt < to)
            .GroupBy(p => p.Method)
            .Select(g => new
            {
                method = g.Key,
                count = g.Count(),
                totalAmount = g.Sum(p => p.Amount)
            })
            .OrderByDescending(x => x.totalAmount)
            .ToListAsync(cancellationToken);

        return Ok(paymentMethods);
    }

    [HttpGet("hourly-sales")]
    public async Task<IActionResult> GetHourlySales([FromQuery] DateTime? date, CancellationToken cancellationToken)
    {
        var targetDate = date ?? DateTime.Today;
        var from = targetDate.Date;
        var to = targetDate.Date.AddDays(1);

        var hourlySales = await _context.Orders
            .Include(o => o.Items)
            .Where(o => o.CreatedAt >= from && o.CreatedAt < to)
            .GroupBy(o => o.CreatedAt.Hour)
            .Select(g => new
            {
                hour = g.Key,
                orderCount = g.Count(),
                revenue = g.SelectMany(o => o.Items).Sum(oi => oi.UnitPrice * oi.Quantity)
            })
            .OrderBy(x => x.hour)
            .ToListAsync(cancellationToken);

        return Ok(hourlySales);
    }

    [HttpGet("table-turnover")]
    public async Task<IActionResult> GetTableTurnover([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, CancellationToken cancellationToken)
    {
        var from = fromDate ?? DateTime.Today.AddDays(-30);
        var to = toDate ?? DateTime.Today.AddDays(1);

        var tableTurnover = await _context.Orders
            .Include(o => o.Table)
            .Include(o => o.Items)
            .Where(o => o.CreatedAt >= from && o.CreatedAt < to && o.TableId != Guid.Empty)
            .GroupBy(o => new { o.TableId, o.Table.Code })
            .Select(g => new
            {
                tableId = g.Key.TableId,
                tableCode = g.Key.Code,
                orderCount = g.Count(),
                totalRevenue = g.SelectMany(o => o.Items).Sum(oi => oi.UnitPrice * oi.Quantity),
                avgOrderValue = g.SelectMany(o => o.Items).Any() 
                    ? g.SelectMany(o => o.Items).Sum(oi => oi.UnitPrice * oi.Quantity) / g.Count()
                    : 0
            })
            .OrderByDescending(x => x.orderCount)
            .ToListAsync(cancellationToken);

        return Ok(tableTurnover);
    }
}
