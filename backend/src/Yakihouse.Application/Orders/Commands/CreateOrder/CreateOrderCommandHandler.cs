using MediatR;
using Microsoft.Extensions.Logging;
using Yakihouse.Application.Common.Interfaces;
using Yakihouse.Application.Common.Models;
using Yakihouse.Application.Kitchen.Services;
using Yakihouse.Application.Orders.DTOs;
using Yakihouse.Domain.Common.Exceptions;
using Yakihouse.Domain.Entities;
using Yakihouse.Domain.Enums;
using Yakihouse.Domain.Repositories;

namespace Yakihouse.Application.Orders.Commands.CreateOrder;

/// <summary>
/// Handler for creating new orders
/// </summary>
public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<OrderDto>>
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<MenuItem> _menuItemRepository;
    private readonly IRepository<MenuOption> _menuOptionRepository;
    private readonly IRepository<DiningTable> _tableRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IKitchenTicketService _kitchenTicketService;
    private readonly IOrderNotificationService _notificationService;
    private readonly ILogger<CreateOrderCommandHandler> _logger;

    public CreateOrderCommandHandler(
        IRepository<Order> orderRepository,
        IRepository<MenuItem> menuItemRepository,
        IRepository<MenuOption> menuOptionRepository,
        IRepository<DiningTable> tableRepository,
        IUnitOfWork unitOfWork,
        IKitchenTicketService kitchenTicketService,
        IOrderNotificationService notificationService,
        ILogger<CreateOrderCommandHandler> logger)
    {
        _orderRepository = orderRepository;
        _menuItemRepository = menuItemRepository;
        _menuOptionRepository = menuOptionRepository;
        _tableRepository = tableRepository;
        _unitOfWork = unitOfWork;
        _kitchenTicketService = kitchenTicketService;
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task<Result<OrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating order for table {TableId} by staff {StaffId}", 
            request.TableId, request.StaffId);

        // Validate table exists
        var table = await _tableRepository.GetByIdAsync(request.TableId, cancellationToken);
        if (table == null)
        {
            _logger.LogWarning("Table {TableId} not found", request.TableId);
            throw new EntityNotFoundException(nameof(DiningTable), request.TableId);
        }

        // Validate table is available
        if (table.Status != TableStatus.Available)
        {
            _logger.LogWarning("Table {TableId} is not available. Current status: {Status}", 
                request.TableId, table.Status);
            return Result<OrderDto>.Failure(
                $"Table {table.Code} is not available. Current status: {table.Status}");
        }

        // Create order
        var order = new Order(request.TableId, request.StaffId, request.GuestCount, request.Notes);
        
        // Update buffet info
        order.UpdateBuffetInfo(
            request.AdultCount,
            request.ChildCount,
            request.ChildHeights,
            request.BuffetType,
            request.HasDessertBuffet);

        // Add items with validation
        var errors = new List<string>();
        foreach (var itemDto in request.Items)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(itemDto.MenuItemId, cancellationToken);
            if (menuItem == null)
            {
                errors.Add($"Menu item {itemDto.MenuItemId} not found");
                continue;
            }

            if (menuItem.Status != MenuItemStatus.Available)
            {
                errors.Add($"Menu item '{menuItem.Name}' is not available");
                continue;
            }

            var orderItem = order.AddItem(
                menuItem.Id,
                menuItem.Name,
                menuItem.BasePrice,
                itemDto.Quantity,
                itemDto.Note);

            // Add options with validation
            foreach (var optionDto in itemDto.Options)
            {
                var menuOption = await _menuOptionRepository.GetByIdAsync(
                    optionDto.MenuOptionId, cancellationToken);
                    
                if (menuOption == null)
                {
                    errors.Add($"Menu option {optionDto.MenuOptionId} not found");
                    continue;
                }

                orderItem.AddOption(
                    menuOption.Id, 
                    menuOption.Name, 
                    menuOption.ExtraPrice, 
                    optionDto.Quantity);
            }
        }

        if (errors.Any())
        {
            _logger.LogWarning("Order creation failed with {ErrorCount} errors", errors.Count);
            return Result<OrderDto>.Failure(errors);
        }

        // Save order
        await _orderRepository.AddAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Order {OrderId} created successfully", order.Id);

        // Submit order and create kitchen tickets
        order.ChangeStatus(OrderStatus.Submitted);
        await _kitchenTicketService.CreateTicketsForOrderAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Kitchen tickets created for order {OrderId}", order.Id);

        // Map to DTO
        var dto = MapToDto(order);

        // Send SignalR notification
        await _notificationService.NotifyOrderCreatedAsync(dto);

        return Result<OrderDto>.Success(dto);
    }

    private static OrderDto MapToDto(Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            TableId = order.TableId,
            TableCode = string.Empty, // Will be populated by query service
            StaffId = order.StaffId,
            StaffName = string.Empty, // Will be populated by query service
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

