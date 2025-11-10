using MediatR;
using Yakihouse.Application.Common.Interfaces;
using Yakihouse.Application.Common.Models;
using Yakihouse.Domain.Repositories;

namespace Yakihouse.Application.Billing.Queries;

public class GetBillByIdQueryHandler : IRequestHandler<GetBillByIdQuery, Result<BillDto>>
{
    private readonly IRepository<Domain.Entities.Bill> _billRepository;

    public GetBillByIdQueryHandler(IRepository<Domain.Entities.Bill> billRepository)
    {
        _billRepository = billRepository;
    }

    public async Task<Result<BillDto>> Handle(GetBillByIdQuery request, CancellationToken cancellationToken)
    {
        var bill = await _billRepository.GetByIdAsync(request.Id, cancellationToken);

        if (bill == null)
        {
            return Result<BillDto>.Failure("Không tìm thấy hóa đơn");
        }
        
        // Note: This is a simplified version. In production, you should use a proper query service
        // that can include related data or use projection

        var billDto = new BillDto(
            Id: bill.Id,
            OrderId: bill.OrderId,
            OrderCode: "", // Order code not available without navigation property loaded
            Subtotal: bill.SubTotal,
            TaxAmount: bill.Tax,
            ServiceChargeAmount: bill.ServiceCharge,
            DiscountAmount: bill.DiscountTotal,
            TotalAmount: bill.GrandTotal,
            Status: bill.Status.ToString(),
            CreatedAt: bill.CreatedAt,
            Items: bill.Order?.Items?.Select(oi => new BillItemDto(
                ItemName: oi.MenuItemName,
                Quantity: oi.Quantity,
                UnitPrice: oi.UnitPrice,
                TotalPrice: oi.UnitPrice * oi.Quantity
            )).ToList() ?? new List<BillItemDto>(),
            Payments: bill.Payments?.Select(p => new BillPaymentDto(
                PaymentMethod: p.Method.ToString(),
                Amount: p.Amount,
                PaidAt: p.PaidAt ?? DateTime.MinValue
            )).ToList() ?? new List<BillPaymentDto>()
        );

        return Result<BillDto>.Success(billDto);
    }
}
