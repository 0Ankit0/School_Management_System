using FluentValidation;
using System;

namespace SMS.Contracts.Users;

public class UpdateUserRequest
{
    public string? Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Guid? RoleId { get; set; }
    public bool? IsActive { get; set; }
}

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Username).EmailAddress().When(x => !string.IsNullOrEmpty(x.Username)).WithMessage("Valid email is required.");
        RuleFor(x => x.FirstName).MaximumLength(50);
        RuleFor(x => x.LastName).MaximumLength(50);
    }
}
