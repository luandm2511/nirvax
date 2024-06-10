using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class Service
{
    public int ServiceId { get; set; }

    public string Name { get; set; } = null!;

    public bool Isdelete { get; set; }

    public virtual ICollection<Advertisement> Advertisements { get; set; } = new List<Advertisement>();
}
