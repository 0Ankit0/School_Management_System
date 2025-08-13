using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMS.Data.Models;

public class AssignmentSubmission
{
    public AssignmentSubmission()
    {
        ExternalId = Guid.NewGuid();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public Guid ExternalId { get; set; }

    [Required]
    public int AssignmentId { get; set; }

    [ForeignKey(nameof(AssignmentId))]
    public Assignment Assignment { get; set; }

    [Required]
    public int StudentId { get; set; }

    [ForeignKey(nameof(StudentId))]
    public Student Student { get; set; }

    public int? FileId { get; set; }

    [ForeignKey(nameof(FileId))]
    public FileStorage File { get; set; }

    [Required]
    public DateTime SubmittedAt { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int CreatedBy { get; set; }
    public int UpdatedBy { get; set; }
}