using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMS.Data.Models;

public class Course
{
    public Course()
    {
        ExternalId = Guid.NewGuid();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public Guid ExternalId { get; set; }

    [Required]
    [MaxLength(20)]
    public string CourseCode { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [MaxLength(500)]
    public string Description { get; set; }

    [Required]
    public int Credits { get; set; }

    [Required]
    public int TeacherId { get; set; }

    [ForeignKey(nameof(TeacherId))]
    public Teacher Teacher { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int CreatedBy { get; set; }
    public int UpdatedBy { get; set; }
}
