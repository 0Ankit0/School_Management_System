using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMS.Data.Models;

public class AuditLog
{
    public AuditLog()
    {
        ExternalId = Guid.NewGuid();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public Guid ExternalId { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; }

    [Required]
    [MaxLength(100)]
    public string Action { get; set; }

    [Required]
    [MaxLength(100)]
    public string Entity { get; set; }

    [Required]
    public int EntityId { get; set; }

    [Required]
    public DateTime Timestamp { get; set; }

    public string Details { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int CreatedBy { get; set; }
    public int UpdatedBy { get; set; }
}
