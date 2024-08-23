using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class OrderDetail
{
    public int OrderId { get; set; }

    public string ProductSizeId { get; set; } = null!;

    public int OdQuantity { get; set; }

    public double OdUnitPrice { get; set; }

    public double OdTuserRate { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual ProductSize ProductSize { get; set; } = null!;
}
