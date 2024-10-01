using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SecureSphere.Models
{
    public class Detection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [ForeignKey("Camera")]
        public int CameraID { get; set; }

        public Camera? Camera { get; set; }

        public DateTime Timestamp { get; set; }

        public bool WeaponType { get; set; }

        public decimal Confidence { get; set; }

        public decimal Status { get; set; }

        [StringLength(50)]
        public string? Reason { get; set; }

        [ForeignKey("ApplicationUser")]
        public string? UserID { get; set; }

        public ApplicationUser? User { get; set; }

        [StringLength(500)]
        public string? ImagePath { get; set;}
    }
}
