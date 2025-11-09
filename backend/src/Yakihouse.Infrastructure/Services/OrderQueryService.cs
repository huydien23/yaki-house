using Microsoft.EntityFrameworkCore;
using Yakihouse.Application.Common.Interfaces;
using Yakihouse.Application.Orders.DTOs;
using Yakihouse.Domain.Enums;
using Yakihouse.Infrastructure.Persistence;

namespace Yakihouse.Infrastructure.Services;

public class OrderQueryService : IOrderQueryService
{
    private readonly YakihouseDbContext _context;

    public OrderQueryService(YakihouseDbContext context)
    {
        _context = context;
    }

    public async Task<OrderDto?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await _context.Orders
            .Include(o => o.Table)
            .Include(o => o.Staff)
            .Include(o => o.Items)
                .ThenInclude(i => i.Options)
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

        if (order == null)
            return null;

        return MapToDto(order);
    }

    public async Task<List<OrderDto>> GetActiveOrdersAsync(CancellationToken cancellationToken = default)
    {
        var orders = await _context.Orders
            .Include(o => o.Table)
            .Include(o => o.Staff)
            .Include(o => o.Items)
                .ThenInclude(i => i.Options)
            .Where(o => o.Status != OrderStatus.Completed && o.Status != OrderStatus.Cancelled)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);

        return orders.Select(MapToDto).ToList();
    }

    public async Task<List<OrderDto>> GetOrdersByTableIdAsync(Guid tableId, CancellationToken cancellationToken = default)
    {
        var orders = await _context.Orders
            .Include(o => o.Table)
            .Include(o => o.Staff)
            .Include(o => o.Items)
                .ThenInclude(i => i.Options)
            .Where(o => o.TableId == tableId && o.Status != OrderStatus.Completed && o.Status != OrderStatus.Cancelled)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);

        return orders.Select(MapToDto).ToList();
    }

    private static OrderDto MapToDto(Domain.Entities.Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            TableId = order.TableId,
            TableCode = order.Table.Code,
            StaffId = order.StaffId,
            StaffName = order.Staff.FullName,
            Status = order.Status.ToString(),
            GuestCount = order.GuestCount,
            AdultCount = order.AdultCount,
            ChildCount = order.ChildCount,
            ChildHeights = order.ChildHeights,
            BuffetType = order.BuffetType,
            HasDessertBuffet = order.HasDessertBuffet,
            Notes = order.Notes,
            CreatedAt = order.CreatedAt,
            ClosedAt = order.ClosedAt,
            Items = order.Items.Select(i => new OrderItemDto
            {
                Id = i.Id,
                MenuItemId = i.MenuItemId,
                MenuItemName = i.MenuItemName,
                UnitPrice = i.UnitPrice,
                Quantity = i.Quantity,
                Note = i.Note,
                Status = i.Status.ToString(),
                Options = i.Options.Select(o => new OrderItemOptionDto
                {
                    Id = o.Id,
                    MenuOptionId = o.MenuOptionId,
                    OptionName = o.OptionName,
                    ExtraPrice = o.ExtraPrice,
                    Quantity = o.Quantity
                }).ToList()
            }).ToList()
        };
    }
}

