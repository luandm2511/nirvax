﻿using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class OrderStatus
{
    public int StatusId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
