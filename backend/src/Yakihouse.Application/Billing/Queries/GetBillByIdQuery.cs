using MediatR;
using Yakihouse.Application.Common.Models;

namespace Yakihouse.Application.Billing.Queries;

public record GetBillByIdQuery(Guid Id) : IRequest<Result<BillDto>>;

public record BillDto(
    Guid Id,
    Guid OrderId,
    string OrderCode,
    decimal Subtotal,
    decimal TaxAmount,
    decimal ServiceChargeAmount,
    decimal DiscountAmount,
    decimal TotalAmount,
    string Status,
    DateTime CreatedAt,
    List<BillItemDto> Items,
    List<BillPaymentDto> Payments
);

public record BillItemDto(
    string ItemName,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice
);

public record BillPaymentDto(
    string PaymentMethod,
    decimal Amount,
    DateTime PaidAt
);
