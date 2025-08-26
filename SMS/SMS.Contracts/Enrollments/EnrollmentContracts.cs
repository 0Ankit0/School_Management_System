using FluentValidation;
using System;

namespace SMS.Contracts.Enrollments;

public class CreateEnrollmentRequest
{
    public Guid StudentExternalId { get; set; }
    public Guid CourseExternalId { get; set; }
}

public class CreateEnrollmentRequestValidator : AbstractValidator<CreateEnrollmentRequest>
{
    public CreateEnrollmentRequestValidator()
    {
        RuleFor(x => x.StudentExternalId).NotEmpty();
        RuleFor(x => x.CourseExternalId).NotEmpty();
    }
}

public class UpdateGradeRequest
{
    public string Grade { get; set; }
}

public class UpdateGradeRequestValidator : AbstractValidator<UpdateGradeRequest>
{
    public UpdateGradeRequestValidator()
    {
        RuleFor(x => x.Grade).NotEmpty().MaximumLength(5);
    }
}

public class EnrollmentResponse
{
    public Guid Id { get; set; }
    public Guid StudentExternalId { get; set; }
    public string StudentFullName { get; set; }
    public Guid CourseExternalId { get; set; }
    public string CourseTitle { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public string Grade { get; set; }
}
