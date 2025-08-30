namespace SMS.Microservices.Library.Models;

public class BookLoan
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public Guid StudentId { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid UpdatedBy { get; set; }
}
