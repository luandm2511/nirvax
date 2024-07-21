using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class WarehouseDetail
{
    public int WarehouseId { get; set; }

    public string ProductSizeId { get; set; } = null!;

    public string Location { get; set; } = null!;

    public virtual ProductSize ProductSize { get; set; } = null!;

    public virtual Warehouse Warehouse { get; set; } = null!;
}
