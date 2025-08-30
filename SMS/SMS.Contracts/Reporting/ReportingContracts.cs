using FluentValidation;
using System;

namespace SMS.Contracts.Reporting;

public class CreateReportRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Query { get; set; }
}

public class CreateReportRequestValidator : AbstractValidator<CreateReportRequest>
{
    public CreateReportRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500);
        RuleFor(x => x.Query).NotEmpty();
    }
}

public class ReportResponse
{
    public Guid Id { get; set; } // Maps to ExternalId
    public string Name { get; set; }
    public string Description { get; set; }
    public string Query { get; set; }
}

public class UpdateReportRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Query { get; set; }
}

public class UpdateReportRequestValidator : AbstractValidator<UpdateReportRequest>
{
    public UpdateReportRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500);
        RuleFor(x => x.Query).NotEmpty();
    }
}
