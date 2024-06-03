using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Warehouse
{
    public int WarehouseId { get; set; }

    public int OwnerId { get; set; }

    public int TotalQuantity { get; set; }

    public double TotalPrice { get; set; }

    public virtual ICollection<ImportProduct> ImportProducts { get; set; } = new List<ImportProduct>();

    public virtual Owner Owner { get; set; } = null!;
}
