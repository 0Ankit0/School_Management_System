using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMS.Data.Models;

public class Announcement
{
    public Announcement()
    {
        ExternalId = Guid.NewGuid();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public Guid ExternalId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [Required]
    [MaxLength(4000)]
    public string Content { get; set; }

    [Required]
    public DateTime PublishDate { get; set; }

    public DateTime? ExpiryDate { get; set; }

    [Required]
    [MaxLength(50)]
    public string TargetAudience { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int CreatedBy { get; set; }
    public int UpdatedBy { get; set; }
}
