using FluentValidation;
using SMS.Contracts.Inventory;

namespace SMS.Microservices.Inventory.Validators;

public class CreateInventoryItemRequestValidator : AbstractValidator<CreateInventoryItemRequest>
{
    public CreateInventoryItemRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500);
        RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0);
        RuleFor(x => x.PurchaseDate).NotEmpty();
        RuleFor(x => x.PurchasePrice).GreaterThanOrEqualTo(0);
    }
}
