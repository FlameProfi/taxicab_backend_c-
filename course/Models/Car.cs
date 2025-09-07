using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace course.Models
{
    [Table("cars")]
    public class Car
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("make")]
        [MaxLength(100)]
        public string Make { get; set; } = string.Empty;

        [Required]
        [Column("model")]
        [MaxLength(100)]
        public string Model { get; set; } = string.Empty;

        [Column("year")]
        public int Year { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("color")]
        [MaxLength(50)]
        public string Color { get; set; } = string.Empty;

        [Column("image_url")]
        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}