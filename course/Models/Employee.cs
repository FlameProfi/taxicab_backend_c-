using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace course.Models
{
    [Table("employees")]
    public class Employee
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("first_name")]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Column("last_name")]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Column("position")]
        [MaxLength(100)]
        public string Position { get; set; } = string.Empty;

        [Column("department")]
        [MaxLength(100)]
        public string Department { get; set; } = string.Empty;

        [Column("email")]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Column("phone")]
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Column("salary")]
        public decimal Salary { get; set; }

        [Column("hire_date")]
        public DateTime HireDate { get; set; }

        [Column("image_url")]
        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}