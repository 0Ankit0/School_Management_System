using FluentValidation;
using System;

namespace SMS.Contracts.ParentGuardians;

public class CreateParentGuardianRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}

public class CreateParentGuardianRequestValidator : AbstractValidator<CreateParentGuardianRequest>
{
    public CreateParentGuardianRequestValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Email).EmailAddress().MaximumLength(100);
        RuleFor(x => x.Phone).MaximumLength(20).Matches(@"^\+?[0-9]{10,15}$").WithMessage("Invalid phone number format.");
    }
}

public class ParentGuardianResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}

public class UpdateParentGuardianRequest
{
    public Guid ExternalId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}

public class UpdateParentGuardianRequestValidator : AbstractValidator<UpdateParentGuardianRequest>
{
    public UpdateParentGuardianRequestValidator()
    {
        RuleFor(x => x.ExternalId).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Email).EmailAddress().MaximumLength(100);
        RuleFor(x => x.Phone).MaximumLength(20).Matches(@"^\+?[0-9]{10,15}$").WithMessage("Invalid phone number format.");
    }
}