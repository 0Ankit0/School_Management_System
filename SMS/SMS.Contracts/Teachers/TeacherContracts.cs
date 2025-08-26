using FluentValidation;
using System;

namespace SMS.Contracts.Teachers;

public class CreateTeacherRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Department { get; set; }
}

public class CreateTeacherRequestValidator : AbstractValidator<CreateTeacherRequest>
{
    public CreateTeacherRequestValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(255);
        RuleFor(x => x.Phone).MaximumLength(30);
        RuleFor(x => x.Department).MaximumLength(100);
    }
}

public class UpdateTeacherRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Department { get; set; }
}

public class UpdateTeacherRequestValidator : AbstractValidator<UpdateTeacherRequest>
{
    public UpdateTeacherRequestValidator()
    {
        RuleFor(x => x.FirstName).MaximumLength(100);
        RuleFor(x => x.LastName).MaximumLength(100);
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrEmpty(x.Email)).MaximumLength(255);
        RuleFor(x => x.Phone).MaximumLength(30);
        RuleFor(x => x.Department).MaximumLength(100);
    }
}

public class TeacherResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Department { get; set; }
}
