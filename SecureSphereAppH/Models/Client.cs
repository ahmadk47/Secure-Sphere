using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecureSphereApp.Models;

public partial class Client
{
    public decimal Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();
}
