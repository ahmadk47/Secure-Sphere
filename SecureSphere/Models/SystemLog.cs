using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SecureSphere.Models
{
    public class SystemLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(50)]
        public string? Details { get; set; }

        [StringLength(15)]
        public string? IpAddress { get; set; }

        [ForeignKey("ApplicationUser")]
        public string? UserID { get; set; }

        public ApplicationUser? User { get; set; }
    }
}
