using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SecureSphereApp.Models;

public partial class Camera
{
    public decimal Id { get; set; }

    public string? Name { get; set; }

    public decimal BranchId { get; set; }

    [Display(Name = "IP Address")]
    public string IpAddress { get; set; } = null!;

    public decimal? Status { get; set; }

    public virtual Branch? Branch { get; set; } = null!;

    public virtual ICollection<Detection> Detections { get; set; } = new List<Detection>();
}
