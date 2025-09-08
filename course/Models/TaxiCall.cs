using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace course.Models
{
    [Table("taxi_calls")]
    public class TaxiCall
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("client_name")]
        [MaxLength(200)]
        public string ClientName { get; set; } = string.Empty;

        [Column("client_phone")]
        [MaxLength(20)]
        public string ClientPhone { get; set; } = string.Empty;

        [Column("call_time")]
        public DateTime CallTime { get; set; }

        [Column("pickup_address")]
        [MaxLength(500)]
        public string PickupAddress { get; set; } = string.Empty;

        [Column("destination_address")]
        [MaxLength(500)]
        public string DestinationAddress { get; set; } = string.Empty;

        [Column("status")]
        [MaxLength(20)]
        public string Status { get; set; } = "pending"; 

        [Column("driver_name")]
        [MaxLength(200)]
        public string? DriverName { get; set; }

        [Column("car_model")]
        [MaxLength(100)]
        public string? CarModel { get; set; }

        [Column("car_number")]
        [MaxLength(20)]
        public string? CarNumber { get; set; }

        [Column("price")]
        public decimal? Price { get; set; }

        [Column("duration")]
        public int? Duration { get; set; } // в минутах

        [Column("distance")]
        public decimal? Distance { get; set; } // в км

        [Column("rating")]
        public int? Rating { get; set; } 

        [Column("notes")]
        [MaxLength(1000)]
        public string? Notes { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}