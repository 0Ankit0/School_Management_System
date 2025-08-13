using FluentValidation;
using System;

namespace SMS.Contracts.Assignments;

public class CreateAssignmentRequest
{
    public Guid CourseExternalId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
}

public class CreateAssignmentRequestValidator : AbstractValidator<CreateAssignmentRequest>
{
    public CreateAssignmentRequestValidator()
    {
        RuleFor(x => x.CourseExternalId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(1000);
        RuleFor(x => x.DueDate).NotEmpty().GreaterThanOrEqualTo(DateTime.Today);
    }
}

public class AssignmentResponse
{
    public Guid Id { get; set; }
    public Guid CourseExternalId { get; set; }
    public string CourseTitle { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
}

public class UpdateAssignmentRequest
{
    public Guid ExternalId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
}

public class UpdateAssignmentRequestValidator : AbstractValidator<UpdateAssignmentRequest>
{
    public UpdateAssignmentRequestValidator()
    {
        RuleFor(x => x.ExternalId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(1000);
        RuleFor(x => x.DueDate).NotEmpty().GreaterThanOrEqualTo(DateTime.Today);
    }
}