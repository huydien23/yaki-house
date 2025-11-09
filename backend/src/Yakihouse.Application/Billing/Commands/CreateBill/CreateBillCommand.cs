using MediatR;
using Yakihouse.Application.Common.Models;
using Yakihouse.Application.Billing.DTOs;

namespace Yakihouse.Application.Billing.Commands.CreateBill;

public record CreateBillCommand : IRequest<Result<BillDto>>
{
    public Guid OrderId { get; init; }
    public bool ApplyPromotions { get; init; } = true; // Tự động áp dụng ưu đãi
}

