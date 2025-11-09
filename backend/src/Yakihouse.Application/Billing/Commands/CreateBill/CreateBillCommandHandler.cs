using MediatR;
using Yakihouse.Application.Billing.DTOs;
using Yakihouse.Application.Billing.Services;
using Yakihouse.Application.Common.Models;
using Yakihouse.Application.Promotions.Services;
using Yakihouse.Domain.Entities;
using Yakihouse.Domain.Enums;
using Yakihouse.Domain.Repositories;

namespace Yakihouse.Application.Billing.Commands.CreateBill;

public class CreateBillCommandHandler : IRequestHandler<CreateBillCommand, Result<BillDto>>
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<Bill> _billRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBillCalculationService _billCalculationService;
    private readonly IPromotionService _promotionService;

    public CreateBillCommandHandler(
        IRepository<Order> orderRepository,
        IRepository<Bill> billRepository,
        IUnitOfWork unitOfWork,
        IBillCalculationService billCalculationService,
        IPromotionService promotionService)
    {
        _orderRepository = orderRepository;
        _billRepository = billRepository;
        _unitOfWork = unitOfWork;
        _billCalculationService = billCalculationService;
        _promotionService = promotionService;
    }

    public async Task<Result<BillDto>> Handle(CreateBillCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order == null)
        {
            return Result<BillDto>.Failure("Order not found");
        }

        // Kiểm tra xem đã có bill chưa
        var existingBills = await _billRepository.FindAsync(b => b.OrderId == request.OrderId, cancellationToken);
        if (existingBills.Any(b => b.Status != BillStatus.Cancelled))
        {
            return Result<BillDto>.Failure("Bill already exists for this order");
        }

        // Tính toán hóa đơn
        var calculationResult = await _billCalculationService.CalculateBillAsync(order, cancellationToken);

        // Áp dụng ưu đãi (nếu có)
        decimal discountTotal = 0m;
        var appliedPromotions = new List<AppliedPromotionDto>();

        if (request.ApplyPromotions)
        {
            var applicablePromotions = await _promotionService.GetApplicablePromotionsAsync(order, cancellationToken);
            
            foreach (var promotion in applicablePromotions.Where(p => p.IsApplicable))
            {
                var discount = await _promotionService.CalculateDiscountAsync(order, promotion.PromotionId, cancellationToken);
                if (discount > 0)
                {
                    discountTotal += discount;
                    appliedPromotions.Add(new AppliedPromotionDto
                    {
                        PromotionId = promotion.PromotionId,
                        Name = promotion.Name,
                        DiscountValue = discount
                    });
                }
            }
        }

        // Tạo Bill
        var bill = new Bill(request.OrderId);
        bill.UpdateAmounts(
            calculationResult.SubTotal,
            discountTotal,
            calculationResult.ServiceCharge,
            calculationResult.Tax);

        // Áp dụng promotions vào bill
        foreach (var promotion in appliedPromotions)
        {
            bill.ApplyPromotion(promotion.PromotionId, promotion.DiscountValue);
        }

        bill.ChangeStatus(BillStatus.PendingPayment);

        await _billRepository.AddAsync(bill, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Map to DTO
        var dto = new BillDto
        {
            Id = bill.Id,
            OrderId = bill.OrderId,
            BuffetTicketTotal = calculationResult.BuffetTicketTotal,
            MenuItemsTotal = calculationResult.MenuItemsTotal,
            SubTotal = calculationResult.SubTotal,
            DiscountTotal = discountTotal,
            ServiceCharge = calculationResult.ServiceCharge,
            Tax = calculationResult.Tax,
            GrandTotal = bill.GrandTotal,
            Status = bill.Status.ToString(),
            CalculationDetails = calculationResult.CalculationDetails,
            AppliedPromotions = appliedPromotions,
            CreatedAt = bill.CreatedAt
        };

        return Result<BillDto>.Success(dto);
    }
}

