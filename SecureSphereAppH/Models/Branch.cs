using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecureSphereApp.Models;

public partial class Branch
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public decimal Id { get; set; }

    public string? Address { get; set; }
    
    public decimal ClientId { get; set; }

    public virtual ICollection<Camera> Cameras { get; set; } = new List<Camera>();

    public virtual Client? Client { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();

}


