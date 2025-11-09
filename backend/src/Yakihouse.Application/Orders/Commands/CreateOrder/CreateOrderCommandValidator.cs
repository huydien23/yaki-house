using FluentValidation;

namespace Yakihouse.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.TableId)
            .NotEmpty().WithMessage("TableId is required");

        RuleFor(x => x.StaffId)
            .NotEmpty().WithMessage("StaffId is required");

        RuleFor(x => x.GuestCount)
            .GreaterThan(0).WithMessage("GuestCount must be greater than 0");

        RuleFor(x => x.AdultCount)
            .GreaterThanOrEqualTo(0).WithMessage("AdultCount must be greater than or equal to 0");

        RuleFor(x => x.ChildCount)
            .GreaterThanOrEqualTo(0).WithMessage("ChildCount must be greater than or equal to 0");

        RuleFor(x => x)
            .Must(x => x.AdultCount + x.ChildCount == x.GuestCount)
            .WithMessage("AdultCount + ChildCount must equal GuestCount");

        RuleFor(x => x.BuffetType)
            .Must(x => x == "Nuong" || x == "NuongLau")
            .WithMessage("BuffetType must be 'Nuong' or 'NuongLau'");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("Order must have at least one item");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(x => x.MenuItemId)
                .NotEmpty().WithMessage("MenuItemId is required");

            item.RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0");
        });
    }
}

