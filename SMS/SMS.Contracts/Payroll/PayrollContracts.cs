using FluentValidation;
using System;

namespace SMS.Contracts.Payroll;

public class CreateSalaryRequest
{
    public Guid TeacherExternalId { get; set; }
    public decimal Amount { get; set; }
    public DateTime EffectiveDate { get; set; }
}

public class CreateSalaryRequestValidator : AbstractValidator<CreateSalaryRequest>
{
    public CreateSalaryRequestValidator()
    {
        RuleFor(x => x.TeacherExternalId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.EffectiveDate).NotEmpty();
    }
}

public class CreateBonusRequest
{
    public Guid TeacherExternalId { get; set; }
    public decimal Amount { get; set; }
    public DateTime BonusDate { get; set; }
}

public class CreateBonusRequestValidator : AbstractValidator<CreateBonusRequest>
{
    public CreateBonusRequestValidator()
    {
        RuleFor(x => x.TeacherExternalId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.BonusDate).NotEmpty();
    }
}

public class CreateDeductionRequest
{
    public Guid TeacherExternalId { get; set; }
    public decimal Amount { get; set; }
    public DateTime DeductionDate { get; set; }
}

public class CreateDeductionRequestValidator : AbstractValidator<CreateDeductionRequest>
{
    public CreateDeductionRequestValidator()
    {
        RuleFor(x => x.TeacherExternalId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.DeductionDate).NotEmpty();
    }
}

public class SalaryResponse
{
    public Guid Id { get; set; } // Maps to ExternalId
    public Guid TeacherExternalId { get; set; }
    public decimal Amount { get; set; }
    public DateTime EffectiveDate { get; set; }
}

public class BonusResponse
{
    public Guid Id { get; set; } // Maps to ExternalId
    public Guid TeacherExternalId { get; set; }
    public decimal Amount { get; set; }
    public DateTime BonusDate { get; set; }
}

public class DeductionResponse
{
    public Guid Id { get; set; } // Maps to ExternalId
    public Guid TeacherExternalId { get; set; }
    public decimal Amount { get; set; }
    public DateTime DeductionDate { get; set; }
}

public class UpdateSalaryRequest
{
    public Guid Id { get; set; }
    public Guid TeacherExternalId { get; set; }
    public decimal Amount { get; set; }
    public DateTime EffectiveDate { get; set; }
}

public class UpdateSalaryRequestValidator : AbstractValidator<UpdateSalaryRequest>
{
    public UpdateSalaryRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.TeacherExternalId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.EffectiveDate).NotEmpty();
    }
}

public class UpdateBonusRequest
{
    public Guid Id { get; set; }
    public Guid TeacherExternalId { get; set; }
    public decimal Amount { get; set; }
    public DateTime BonusDate { get; set; }
}

public class UpdateBonusRequestValidator : AbstractValidator<UpdateBonusRequest>
{
    public UpdateBonusRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.TeacherExternalId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.BonusDate).NotEmpty();
    }
}

public class UpdateDeductionRequest
{
    public Guid Id { get; set; }
    public Guid TeacherExternalId { get; set; }
    public decimal Amount { get; set; }
    public DateTime DeductionDate { get; set; }
}

public class UpdateDeductionRequestValidator : AbstractValidator<UpdateDeductionRequest>
{
    public UpdateDeductionRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.TeacherExternalId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.DeductionDate).NotEmpty();
    }
}
