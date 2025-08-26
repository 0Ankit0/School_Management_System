using FluentValidation;
using System;

namespace SMS.Contracts.Terms;

public class CreateTermRequest
{
    public Guid AcademicYearExternalId { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class CreateTermRequestValidator : AbstractValidator<CreateTermRequest>
{
    public CreateTermRequestValidator()
    {
        RuleFor(x => x.AcademicYearExternalId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty().GreaterThan(x => x.StartDate);
    }
}

public class UpdateTermRequest
{
    public string? Name { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class UpdateTermRequestValidator : AbstractValidator<UpdateTermRequest>
{
    public UpdateTermRequestValidator()
    {
        RuleFor(x => x.Name).MaximumLength(50);
        RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate).When(x => x.StartDate.HasValue && x.EndDate.HasValue);
    }
}

public class TermResponse
{
    public Guid Id { get; set; }
    public Guid AcademicYearExternalId { get; set; }
    public string AcademicYearName { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
