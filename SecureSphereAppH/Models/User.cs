using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SecureSphereApp.Models;

public partial class User
{
    public decimal Id { get; set; }

    public string? Name { get; set; } = null!;

    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Display(Name="Account Creation Time")]
    public DateTime CreateAt { get; set; }

    public decimal BranchId { get; set; }

    public virtual Branch? Branch { get; set; } = null!;

    public virtual ICollection<Detection> Detections { get; set; } = new List<Detection>();

    public virtual ICollection<SystemLog> SystemLogs { get; set; } = new List<SystemLog>();
}
