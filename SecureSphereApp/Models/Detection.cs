using System;
using System.Collections.Generic;

namespace SecureSphereApp.Models;

public partial class Detection
{
    public decimal Id { get; set; }

    public decimal CameraId { get; set; }

    public DateTime Timestamp { get; set; }

    public bool WeaponType { get; set; }

    public decimal Confidence { get; set; }

    public decimal Status { get; set; }

    public string? Reason { get; set; }

    public decimal UserId { get; set; }

    public virtual Camera? Camera { get; set; } = null!;

    public virtual User? User { get; set; } = null!;
}
