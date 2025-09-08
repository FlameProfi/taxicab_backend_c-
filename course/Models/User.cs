using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace course.Models
{
    public enum UserRole
    {
        Admin,
        Manager,
        Dispatcher,
        Driver
    }

    [Table("users")]
    public class User
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("username")]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [Column("email")]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Column("password_hash")]
        public string PasswordHash { get; set; } = string.Empty;

        [Column("role")]
        public UserRole Role { get; set; } = UserRole.Dispatcher;

        [Column("first_name")]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Column("last_name")]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Column("phone")]
        [MaxLength(20)]
        public string? Phone { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}