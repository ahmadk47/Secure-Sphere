using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SecureSphere.Models
{
    public class Branch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(100)]
        public string? Address { get; set; }

        [ForeignKey("Client")]
        [DisplayName("Client")]
        public int ClientID { get; set; }

        public Client? Client { get; set; }

        public ICollection<Camera> Cameras { get; set; } = new List<Camera>();
        public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    }
}
