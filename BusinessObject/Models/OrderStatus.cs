using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class OrderStatus
{
    public int StatusId { get; set; }

    public string OrderStatusName { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
