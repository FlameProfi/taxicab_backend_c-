using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace course.Models
{
    [Table("driver_applications")]
    public class DriverApplication
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("full_name")]
        [MaxLength(200)]
        public string FullName { get; set; } = string.Empty;

        [Column("phone")]
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Column("email")]
        [MaxLength(255)]
        public string? Email { get; set; }

        [Column("driving_experience")]
        public int DrivingExperience { get; set; } // в годах

        [Column("car_model")]
        [MaxLength(100)]
        public string? CarModel { get; set; }

        [Column("car_year")]
        public int? CarYear { get; set; }

        [Column("message")]
        [MaxLength(1000)]
        public string Message { get; set; } = string.Empty;

        [Column("status")]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}