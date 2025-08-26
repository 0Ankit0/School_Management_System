using FluentValidation;
using System;

namespace SMS.Contracts.Messages;

public class CreateMessageRequest
{
    public Guid RecipientExternalId { get; set; }
    public string? Content { get; set; }
}

public class CreateMessageRequestValidator : AbstractValidator<CreateMessageRequest>
{
    public CreateMessageRequestValidator()
    {
        RuleFor(x => x.RecipientExternalId).NotEmpty();
        RuleFor(x => x.Content).NotEmpty().MaximumLength(2000);
    }
}

public class MessageResponse
{
    public Guid Id { get; set; }
    public Guid SenderExternalId { get; set; }
    public string? SenderFullName { get; set; }
    public Guid RecipientExternalId { get; set; }
    public string? RecipientFullName { get; set; }
    public string? Content { get; set; }
    public DateTime SentAt { get; set; }
    public bool IsRead { get; set; }
}