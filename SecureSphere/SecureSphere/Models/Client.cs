using System.ComponentModel.DataAnnotations;

namespace SecureSphere.Models
{
    public class Client
    {
        [Key]
        public decimal ID { get; set; }

        [StringLength(50)]
        public required string Name { get; set; }

        public ICollection<Branch>? Branches { get; set; } = new List<Branch>();
    }
}
