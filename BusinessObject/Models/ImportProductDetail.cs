using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class ImportProductDetail
{
    public int ImportId { get; set; }

    public string ProductSizeId { get; set; } = null!;

    public int QuantityReceived { get; set; }

    public double UnitPrice { get; set; }

    public virtual ImportProduct Import { get; set; } = null!;

    public virtual ProductSize ProductSize { get; set; } = null!;
}
