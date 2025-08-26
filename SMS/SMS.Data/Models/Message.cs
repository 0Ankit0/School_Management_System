using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMS.Data.Models;

public class Message
{
    public Message()
    {
        ExternalId = Guid.NewGuid();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public Guid ExternalId { get; set; }

    [Required]
    public int SenderId { get; set; }

    [ForeignKey(nameof(SenderId))]
    public User Sender { get; set; }

    [Required]
    public int RecipientId { get; set; }

    [ForeignKey(nameof(RecipientId))]
    public User Recipient { get; set; }

    [Required]
    [MaxLength(2000)]
    public string Content { get; set; }

    [Required]
    public DateTime SentAt { get; set; }

    [Required]
    public bool IsRead { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int CreatedBy { get; set; }
    public int UpdatedBy { get; set; }
}
