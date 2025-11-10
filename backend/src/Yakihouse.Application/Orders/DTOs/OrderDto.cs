namespace Yakihouse.Application.Orders.DTOs;

public record OrderDto
{
    public Guid Id { get; init; }
    public Guid TableId { get; init; }
    public string TableCode { get; init; } = string.Empty;
    public Guid StaffId { get; init; }
    public string StaffName { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public int GuestCount { get; init; }
    public int AdultCount { get; init; }
    public int ChildCount { get; init; }
    public string? ChildHeights { get; init; }
    public string BuffetType { get; init; } = "Nuong";
    public bool HasDessertBuffet { get; init; }
    public string? Notes { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? ClosedAt { get; init; }
    public List<OrderItemDto> Items { get; init; } = new();
}

public record OrderItemDto
{
    public Guid Id { get; init; }
    public Guid MenuItemId { get; init; }
    public string MenuItemName { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
    public int Quantity { get; init; }
    public string? Note { get; init; }
    public string Status { get; init; } = string.Empty;
    public List<OrderItemOptionDto> Options { get; init; } = new();
}

public record OrderItemOptionDto
{
    public Guid Id { get; init; }
    public Guid MenuOptionId { get; init; }
    public string OptionName { get; init; } = string.Empty;
    public decimal ExtraPrice { get; init; }
    public int Quantity { get; init; }
}

