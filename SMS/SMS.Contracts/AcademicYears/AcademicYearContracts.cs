using FluentValidation;
using System;

namespace SMS.Contracts.AcademicYears;

public class CreateAcademicYearRequest
{
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class CreateAcademicYearRequestValidator : AbstractValidator<CreateAcademicYearRequest>
{
    public CreateAcademicYearRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty().GreaterThan(x => x.StartDate);
    }
}

public class UpdateAcademicYearRequest
{
    public string? Name { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool? IsActive { get; set; }
}

public class UpdateAcademicYearRequestValidator : AbstractValidator<UpdateAcademicYearRequest>
{
    public UpdateAcademicYearRequestValidator()
    {
        RuleFor(x => x.Name).MaximumLength(50);
        RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate).When(x => x.StartDate.HasValue && x.EndDate.HasValue);
    }
}

public class AcademicYearResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
}
