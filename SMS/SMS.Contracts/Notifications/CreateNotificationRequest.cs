using FluentValidation;
using System;

namespace SMS.Contracts.Notifications;

public class CreateNotificationRequest
{
    public Guid UserExternalId { get; set; }
    public string Content { get; set; }
    public string Type { get; set; }
}

public class CreateNotificationRequestValidator : AbstractValidator<CreateNotificationRequest>
{
    public CreateNotificationRequestValidator()
    {
        RuleFor(x => x.UserExternalId).NotEmpty();
        RuleFor(x => x.Content).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.Type).NotEmpty().MaximumLength(50);
    }
}
