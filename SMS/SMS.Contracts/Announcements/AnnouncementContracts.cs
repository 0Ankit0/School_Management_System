using FluentValidation;
using System;

namespace SMS.Contracts.Announcements;

public class CreateAnnouncementRequest
{
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string TargetAudience { get; set; }
}

public class CreateAnnouncementRequestValidator : AbstractValidator<CreateAnnouncementRequest>
{
    public CreateAnnouncementRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Content).NotEmpty().MaximumLength(4000);
        RuleFor(x => x.PublishDate).NotEmpty().LessThanOrEqualTo(DateTime.Now);
        RuleFor(x => x.ExpiryDate).GreaterThanOrEqualTo(x => x.PublishDate).When(x => x.ExpiryDate.HasValue);
        RuleFor(x => x.TargetAudience).NotEmpty().MaximumLength(50);
    }
}

public class AnnouncementResponse
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? TargetAudience { get; set; }
}