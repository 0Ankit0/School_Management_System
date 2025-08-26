using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMS.Data.Models;

public class FileStorage
{
    public FileStorage()
    {
        ExternalId = Guid.NewGuid();
        Metadata = "{}"; // Initialize as empty JSON object
        Version = 1; // Initial version
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public Guid ExternalId { get; set; }

    [Required]
    [MaxLength(255)]
    public string FileName { get; set; }

    [Required]
    [MaxLength(1000)]
    public string FilePath { get; set; }

    [Required]
    public int UploadedBy { get; set; }

    // Note: User is not directly part of this microservice's domain. 
    // In a real microservices setup, this would be a foreign key to the User service's internal ID.
    // For now, we'll keep it as int and assume it maps to a user in the User service.
    // public User Uploader { get; set; } // Removed direct navigation property to avoid cross-service model dependency

    [Required]
    public DateTime UploadedAt { get; set; }

    [Required]
    [MaxLength(100)]
    public string ContentType { get; set; }

    [Required]
    public long Length { get; set; }

    // Storing metadata as JSON string for flexibility. Requires JSON column support in DB or manual serialization/deserialization.
    public string Metadata { get; set; }

    [Required]
    public int Version { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int CreatedBy { get; set; }
    public int UpdatedBy { get; set; }
}
