using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMS.Data.Models;

public class User
{
    public User()
    {
        ExternalId = Guid.NewGuid();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public Guid ExternalId { get; set; }
    [Required]
    [MaxLength(50)]
    public string Username { get; set; }
    [Required]
    public string PasswordHash { get; set; }
    [Required]
    public string Salt { get; set; }
    [Required]
    public int RoleId { get; set; }
    [ForeignKey(nameof(RoleId))]
    public Role Role { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int CreatedBy { get; set; }
    public int UpdatedBy { get; set; }
}
