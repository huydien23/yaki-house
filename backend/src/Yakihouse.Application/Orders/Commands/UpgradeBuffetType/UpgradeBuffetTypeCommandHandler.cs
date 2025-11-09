using MediatR;
using Yakihouse.Application.Common.Models;
using Yakihouse.Domain.Repositories;

namespace Yakihouse.Application.Orders.Commands.UpgradeBuffetType;

public class UpgradeBuffetTypeCommandHandler : IRequestHandler<UpgradeBuffetTypeCommand, Result>
{
    private readonly IRepository<Domain.Entities.Order> _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpgradeBuffetTypeCommandHandler(
        IRepository<Domain.Entities.Order> orderRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpgradeBuffetTypeCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order == null)
        {
            return Result.Failure("Order not found");
        }

        if (order.BuffetType != "Nuong")
        {
            return Result.Failure("Chỉ có thể nâng cấp từ vé Buffet Nướng (175k) lên vé Buffet Nướng Lẩu (199k)");
        }

        order.UpgradeBuffetType();
        order.RecordAudit("BuffetUpgraded", request.ActorId, "Nâng cấp từ vé Nướng lên vé Nướng Lẩu");

        _orderRepository.Update(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

