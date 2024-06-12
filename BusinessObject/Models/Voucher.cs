using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Voucher
{
    public string VoucherId { get; set; } = null!;

    public double Price { get; set; }

    public int Quantity { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int OwnerId { get; set; }

    public bool Isdelete { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Owner Owner { get; set; } = null!;
}
