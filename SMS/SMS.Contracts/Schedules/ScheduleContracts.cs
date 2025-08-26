using FluentValidation;
using System;

namespace SMS.Contracts.Schedules;

public class CreateScheduleRequest
{
    public Guid ClassExternalId { get; set; }
    public Guid SubjectExternalId { get; set; }
    public Guid TeacherExternalId { get; set; }
    public Guid? TermExternalId { get; set; }
    public string? Room { get; set; }
    public int? DayOfWeek { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class CreateScheduleRequestValidator : AbstractValidator<CreateScheduleRequest>
{
    public CreateScheduleRequestValidator()
    {
        RuleFor(x => x.ClassExternalId).NotEmpty();
        RuleFor(x => x.SubjectExternalId).NotEmpty();
        RuleFor(x => x.TeacherExternalId).NotEmpty();
        RuleFor(x => x.Room).MaximumLength(50);
        RuleFor(x => x.DayOfWeek).InclusiveBetween(0, 6).When(x => x.DayOfWeek.HasValue);
        RuleFor(x => x.EndTime).GreaterThan(x => x.StartTime).When(x => x.StartTime.HasValue && x.EndTime.HasValue);
    }
}

public class UpdateScheduleRequest
{
    public string? Room { get; set; }
    public int? DayOfWeek { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class UpdateScheduleRequestValidator : AbstractValidator<UpdateScheduleRequest>
{
    public UpdateScheduleRequestValidator()
    {
        RuleFor(x => x.Room).MaximumLength(50);
        RuleFor(x => x.DayOfWeek).InclusiveBetween(0, 6).When(x => x.DayOfWeek.HasValue);
        RuleFor(x => x.EndTime).GreaterThan(x => x.StartTime).When(x => x.StartTime.HasValue && x.EndTime.HasValue);
    }
}

public class ScheduleResponse
{
    public Guid Id { get; set; }
    public Guid ClassExternalId { get; set; }
    public string ClassName { get; set; }
    public Guid SubjectExternalId { get; set; }
    public string SubjectName { get; set; }
    public Guid TeacherExternalId { get; set; }
    public string TeacherFullName { get; set; }
    public Guid? TermExternalId { get; set; }
    public string? TermName { get; set; }
    public string? Room { get; set; }
    public int? DayOfWeek { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
