using FluentValidation;
using System;

namespace SMS.Contracts.Classes;

public class CreateClassRequest
{
    public Guid AcademicYearExternalId { get; set; }
    public string Name { get; set; }
    public int Capacity { get; set; }
    public Guid? HomeroomTeacherExternalId { get; set; }
}

public class CreateClassRequestValidator : AbstractValidator<CreateClassRequest>
{
    public CreateClassRequestValidator()
    {
        RuleFor(x => x.AcademicYearExternalId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Capacity).GreaterThan(0);
    }
}

public class UpdateClassRequest
{
    public string? Name { get; set; }
    public int? Capacity { get; set; }
    public Guid? HomeroomTeacherExternalId { get; set; }
}

public class UpdateClassRequestValidator : AbstractValidator<UpdateClassRequest>
{
    public UpdateClassRequestValidator()
    {
        RuleFor(x => x.Name).MaximumLength(100);
        RuleFor(x => x.Capacity).GreaterThan(0).When(x => x.Capacity.HasValue);
    }
}

public class ClassResponse
{
    public Guid Id { get; set; }
    public Guid AcademicYearExternalId { get; set; }
    public string AcademicYearName { get; set; }
    public string Name { get; set; }
    public int Capacity { get; set; }
    public Guid? HomeroomTeacherExternalId { get; set; }
    public string? HomeroomTeacherFullName { get; set; }
}
