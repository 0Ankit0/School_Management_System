using FluentValidation;
using System;

namespace SMS.Contracts.Subjects;

public class CreateSubjectRequest
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
}

public class CreateSubjectRequestValidator : AbstractValidator<CreateSubjectRequest>
{
    public CreateSubjectRequestValidator()
    {
        RuleFor(x => x.Code).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Description).MaximumLength(500);
    }
}

public class UpdateSubjectRequest
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}

public class UpdateSubjectRequestValidator : AbstractValidator<UpdateSubjectRequest>
{
    public UpdateSubjectRequestValidator()
    {
        RuleFor(x => x.Code).MaximumLength(20);
        RuleFor(x => x.Name).MaximumLength(255);
        RuleFor(x => x.Description).MaximumLength(500);
    }
}

public class SubjectResponse
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
}
