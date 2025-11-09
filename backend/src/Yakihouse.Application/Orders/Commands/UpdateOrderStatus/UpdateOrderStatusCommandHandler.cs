using MediatR;
using Yakihouse.Application.Common.Models;
using Yakihouse.Domain.Entities;
using Yakihouse.Domain.Enums;
using Yakihouse.Domain.Repositories;

namespace Yakihouse.Application.Orders.Commands.UpdateOrderStatus;

public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, Result>
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateOrderStatusCommandHandler(
        IRepository<Order> orderRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order == null)
        {
            return Result.Failure("Order not found");
        }

        if (!Enum.TryParse<OrderStatus>(request.Status, out var status))
        {
            return Result.Failure($"Invalid status: {request.Status}");
        }

        order.ChangeStatus(status);
        order.RecordAudit("StatusChanged", request.ActorId, $"Status changed to {status}");

        _orderRepository.Update(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

