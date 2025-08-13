using FluentValidation;
using System;

namespace SMS.Contracts.Courses;

public class CreateCourseRequest
{
    public string CourseCode { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Credits { get; set; }
    public Guid TeacherExternalId { get; set; }
}

public class CreateCourseRequestValidator : AbstractValidator<CreateCourseRequest>
{
    public CreateCourseRequestValidator()
    {
        RuleFor(x => x.CourseCode).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500);
        RuleFor(x => x.Credits).GreaterThan(0).WithMessage("Credits must be greater than 0.");
        RuleFor(x => x.TeacherExternalId).NotEmpty().WithMessage("Teacher is required.");
    }
}

public class CourseResponse
{
    public Guid Id { get; set; }
    public string CourseCode { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Credits { get; set; }
    public Guid TeacherExternalId { get; set; }
    public string TeacherFullName { get; set; }
}