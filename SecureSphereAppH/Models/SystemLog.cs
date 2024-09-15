using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SecureSphereApp.Models;

public partial class SystemLog
{
    public decimal Id { get; set; }

    public string Details { get; set; } = null!;

    [Display(Name ="IP Address")]
    public string IpAddress { get; set; } = null!;

    public decimal UserId { get; set; }

    public virtual User? User { get; set; } = null!;
}
