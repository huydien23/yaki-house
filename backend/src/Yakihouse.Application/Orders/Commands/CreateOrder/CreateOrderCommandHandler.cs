using MediatR;
using Yakihouse.Application.Common.Models;
using Yakihouse.Application.Kitchen.Services;
using Yakihouse.Application.Orders.DTOs;
using Yakihouse.Domain.Entities;
using Yakihouse.Domain.Enums;
using Yakihouse.Domain.Repositories;

namespace Yakihouse.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<OrderDto>>
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<MenuItem> _menuItemRepository;
    private readonly IRepository<MenuOption> _menuOptionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IKitchenTicketService _kitchenTicketService;

    public CreateOrderCommandHandler(
        IRepository<Order> orderRepository,
        IRepository<MenuItem> menuItemRepository,
        IRepository<MenuOption> menuOptionRepository,
        IUnitOfWork unitOfWork,
        IKitchenTicketService kitchenTicketService)
    {
        _orderRepository = orderRepository;
        _menuItemRepository = menuItemRepository;
        _menuOptionRepository = menuOptionRepository;
        _unitOfWork = unitOfWork;
        _kitchenTicketService = kitchenTicketService;
    }

    public async Task<Result<OrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // Create order
        var order = new Order(request.TableId, request.StaffId, request.GuestCount, request.Notes);
        
        // Update buffet info
        order.UpdateBuffetInfo(
            request.AdultCount,
            request.ChildCount,
            request.ChildHeights,
            request.BuffetType,
            request.HasDessertBuffet);

        // Add items
        foreach (var itemDto in request.Items)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(itemDto.MenuItemId, cancellationToken);
            if (menuItem == null)
            {
                return Result<OrderDto>.Failure($"Menu item {itemDto.MenuItemId} not found");
            }

            var orderItem = order.AddItem(
                menuItem.Id,
                menuItem.Name,
                menuItem.BasePrice,
                itemDto.Quantity,
                itemDto.Note);

            // Add options
            foreach (var optionDto in itemDto.Options)
            {
                var menuOption = await _menuOptionRepository.GetByIdAsync(optionDto.MenuOptionId, cancellationToken);
                if (menuOption == null)
                {
                    return Result<OrderDto>.Failure($"Menu option {optionDto.MenuOptionId} not found");
                }

                orderItem.AddOption(menuOption.Id, menuOption.Name, menuOption.ExtraPrice, optionDto.Quantity);
            }
        }

        await _orderRepository.AddAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Submit order và tạo KitchenTickets
        order.ChangeStatus(OrderStatus.Submitted);
        await _kitchenTicketService.CreateTicketsForOrderAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Map to DTO
        var dto = new OrderDto
        {
            Id = order.Id,
            TableId = order.TableId,
            TableCode = "", // Will be populated by query
            StaffId = order.StaffId,
            StaffName = "", // Will be populated by query
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

        return Result<OrderDto>.Success(dto);
    }
}

