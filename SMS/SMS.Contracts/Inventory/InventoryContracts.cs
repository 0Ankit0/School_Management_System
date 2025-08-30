using FluentValidation;
using System;

namespace SMS.Contracts.Inventory;

public class CreateInventoryItemRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal PurchasePrice { get; set; }
}

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

public class InventoryItemResponse
{
    public Guid Id { get; set; } // Maps to ExternalId
    public string Name { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal PurchasePrice { get; set; }
}

public class UpdateInventoryItemRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal PurchasePrice { get; set; }
}

public class UpdateInventoryItemRequestValidator : AbstractValidator<UpdateInventoryItemRequest>
{
    public UpdateInventoryItemRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500);
        RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0);
        RuleFor(x => x.PurchaseDate).NotEmpty();
        RuleFor(x => x.PurchasePrice).GreaterThanOrEqualTo(0);
    }
}
