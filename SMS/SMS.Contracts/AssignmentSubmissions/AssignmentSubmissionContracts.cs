using FluentValidation;
using System;

namespace SMS.Contracts.AssignmentSubmissions;

public class CreateAssignmentSubmissionRequest
{
    public Guid AssignmentExternalId { get; set; }
    public Guid StudentExternalId { get; set; }
    public string FileName { get; set; }
}

public class CreateAssignmentSubmissionRequestValidator : AbstractValidator<CreateAssignmentSubmissionRequest>
{
    public CreateAssignmentSubmissionRequestValidator()
    {
        RuleFor(x => x.AssignmentExternalId).NotEmpty();
        RuleFor(x => x.StudentExternalId).NotEmpty();
        RuleFor(x => x.FileName).NotEmpty().MaximumLength(255);
    }
}

public class AssignmentSubmissionResponse
{
    public Guid Id { get; set; }
    public Guid AssignmentExternalId { get; set; }
    public string? AssignmentTitle { get; set; }
    public Guid StudentExternalId { get; set; }
    public string? StudentFullName { get; set; }
    public string? FileName { get; set; }
    public DateTime SubmittedAt { get; set; }
}