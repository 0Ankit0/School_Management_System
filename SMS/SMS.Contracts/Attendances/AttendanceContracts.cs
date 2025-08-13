using FluentValidation;
using System;

namespace SMS.Contracts.Attendances;

public class CreateAttendanceRequest
{
    public Guid StudentExternalId { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; }
    public string Notes { get; set; }
}

public class CreateAttendanceRequestValidator : AbstractValidator<CreateAttendanceRequest>
{
    public CreateAttendanceRequestValidator()
    {
        RuleFor(x => x.StudentExternalId).NotEmpty();
        RuleFor(x => x.Date).NotEmpty().LessThanOrEqualTo(DateTime.Now);
        RuleFor(x => x.Status).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Notes).MaximumLength(500);
    }
}

public class AttendanceResponse
{
    public Guid Id { get; set; }
    public Guid StudentExternalId { get; set; }
    public string StudentFullName { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; }
    public string Notes { get; set; }
}

public class UpdateAttendanceRequest
{
    public Guid ExternalId { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; }
    public string Notes { get; set; }
}

public class UpdateAttendanceRequestValidator : AbstractValidator<UpdateAttendanceRequest>
{
    public UpdateAttendanceRequestValidator()
    {
        RuleFor(x => x.ExternalId).NotEmpty();
        RuleFor(x => x.Date).NotEmpty().LessThanOrEqualTo(DateTime.Now);
        RuleFor(x => x.Status).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Notes).MaximumLength(500);
    }
}