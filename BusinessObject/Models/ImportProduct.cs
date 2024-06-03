using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class ImportProduct
{
    public int ImportId { get; set; }

    public int WarehouseId { get; set; }

    public DateTime ImportDate { get; set; }

    public string Origin { get; set; } = null!;

    public int Quantity { get; set; }

    public double TotalPrice { get; set; }

    public virtual Warehouse Warehouse { get; set; } = null!;
}
