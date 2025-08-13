using FluentValidation;
using System;

namespace SMS.Contracts.Students;

public class CreateStudentRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
}

public class CreateStudentRequestValidator : AbstractValidator<CreateStudentRequest>
{
    public CreateStudentRequestValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.DateOfBirth).LessThan(DateTime.Now.AddYears(-5)).WithMessage("Student must be at least 5 years old.");
        RuleFor(x => x.Gender).NotEmpty().MaximumLength(10);
        RuleFor(x => x.Address).MaximumLength(255);
        RuleFor(x => x.Phone).MaximumLength(20).Matches(@"^\+?[0-9]{10,15}$").WithMessage("Invalid phone number format.");
        RuleFor(x => x.Email).EmailAddress().MaximumLength(100);
    }
}

public class StudentResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
}