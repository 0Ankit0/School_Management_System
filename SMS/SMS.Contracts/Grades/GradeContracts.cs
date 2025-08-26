using FluentValidation;
using System;

namespace SMS.Contracts.Grades;

public class CreateGradeRequest
{
    public Guid? EnrollmentExternalId { get; set; }
    public Guid? AssignmentExternalId { get; set; }
    public Guid StudentExternalId { get; set; }
    public Guid? CourseExternalId { get; set; }
    public Guid? TermExternalId { get; set; }
    public decimal Score { get; set; }
    public decimal MaxScore { get; set; }
    public string? Letter { get; set; }
    public decimal Weight { get; set; }
    public string? Comment { get; set; }
}

public class CreateGradeRequestValidator : AbstractValidator<CreateGradeRequest>
{
    public CreateGradeRequestValidator()
    {
        RuleFor(x => x.StudentExternalId).NotEmpty();
        RuleFor(x => x.Score).InclusiveBetween(0, 1000); // Assuming max score can be up to 1000
        RuleFor(x => x.MaxScore).GreaterThan(0);
        RuleFor(x => x.Weight).InclusiveBetween(0, 1);
        RuleFor(x => x.EnrollmentExternalId).NotEmpty().When(x => !x.AssignmentExternalId.HasValue).WithMessage("Either EnrollmentExternalId or AssignmentExternalId must be provided.");
        RuleFor(x => x.AssignmentExternalId).NotEmpty().When(x => !x.EnrollmentExternalId.HasValue).WithMessage("Either EnrollmentExternalId or AssignmentExternalId must be provided.");
    }
}

public class UpdateGradeRequest
{
    public decimal? Score { get; set; }
    public decimal? MaxScore { get; set; }
    public string? Letter { get; set; }
    public decimal? Weight { get; set; }
    public string? Comment { get; set; }
}

public class UpdateGradeRequestValidator : AbstractValidator<UpdateGradeRequest>
{
    public UpdateGradeRequestValidator()
    {
        RuleFor(x => x.Score).InclusiveBetween(0, 1000).When(x => x.Score.HasValue);
        RuleFor(x => x.MaxScore).GreaterThan(0).When(x => x.MaxScore.HasValue);
        RuleFor(x => x.Weight).InclusiveBetween(0, 1).When(x => x.Weight.HasValue);
    }
}

public class GradeResponse
{
    public Guid Id { get; set; }
    public Guid? EnrollmentExternalId { get; set; }
    public Guid? AssignmentExternalId { get; set; }
    public Guid StudentExternalId { get; set; }
    public string StudentFullName { get; set; }
    public Guid? CourseExternalId { get; set; }
    public string? CourseTitle { get; set; }
    public Guid? TermExternalId { get; set; }
    public string? TermName { get; set; }
    public decimal Score { get; set; }
    public decimal MaxScore { get; set; }
    public string? Letter { get; set; }
    public decimal Weight { get; set; }
    public string? Comment { get; set; }
    public DateTime RecordedAt { get; set; }
}
