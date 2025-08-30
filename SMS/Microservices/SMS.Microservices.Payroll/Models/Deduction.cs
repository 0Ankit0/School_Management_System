namespace SMS.Microservices.Payroll.Models;

public class Deduction
{
    public Guid Id { get; set; }
    public Guid TeacherId { get; set; }
    public decimal Amount { get; set; }
    public DateTime DeductionDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid UpdatedBy { get; set; }
}
