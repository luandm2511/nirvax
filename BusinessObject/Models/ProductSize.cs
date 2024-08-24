using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class ProductSize
{
    public string ProductSizeId { get; set; } = null!;

    public int SizeId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public bool Isdelete { get; set; }

    public virtual ICollection<ImportProductDetail> ImportProductDetails { get; set; } = new List<ImportProductDetail>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Product Product { get; set; } = null!;

    public virtual Size Size { get; set; } = null!;
}
