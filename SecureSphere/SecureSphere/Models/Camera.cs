using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SecureSphere.Models
{
    public class Camera
    {
        [Key]
        public decimal ID { get; set; }

        [StringLength(50)]
        public required string Name { get; set; }

        [ForeignKey("Branch")]
        public decimal BranchID { get; set; }

        public Branch? Branch { get; set; }

        [StringLength(15)]
        public string IpAddress { get; set; } = string.Empty;

        public decimal? Status { get; set; }

        public ICollection<Detection>? Detections { get; set; } = new List<Detection>();
    }
}
