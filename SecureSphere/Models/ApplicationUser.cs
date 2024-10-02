using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecureSphere.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? ProfilePictureUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey("Branch")]
        public int BranchID { get; set; }

        public Branch? Branch { get; set; }

        public ICollection<Detection>? Detections { get; set; }
        public ICollection<SystemLog>? SystemLogs { get; set; }
    }
}
