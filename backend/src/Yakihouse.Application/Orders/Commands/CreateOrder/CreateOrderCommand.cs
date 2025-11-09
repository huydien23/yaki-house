using MediatR;
using Yakihouse.Application.Common.Models;
using Yakihouse.Application.Orders.DTOs;

namespace Yakihouse.Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand : IRequest<Result<OrderDto>>
{
    public Guid TableId { get; init; }
    public Guid StaffId { get; init; }
    public int GuestCount { get; init; }
    public int AdultCount { get; init; }
    public int ChildCount { get; init; }
    public string? ChildHeights { get; init; } // JSON array: [1.1, 1.2, 0.9]
    public string BuffetType { get; init; } = "Nuong"; // "Nuong" hoặc "NuongLau"
    public bool HasDessertBuffet { get; init; }
    public string? Notes { get; init; }
    public List<CreateOrderItemDto> Items { get; init; } = new();
}

public record CreateOrderItemDto
{
    public Guid MenuItemId { get; init; }
    public int Quantity { get; init; }
    public string? Note { get; init; }
    public List<CreateOrderItemOptionDto> Options { get; init; } = new();
}

public record CreateOrderItemOptionDto
{
    public Guid MenuOptionId { get; init; }
    public int Quantity { get; init; }
}

