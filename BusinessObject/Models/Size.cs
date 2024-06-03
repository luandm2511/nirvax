using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Size
{
    public int SizeId { get; set; }

    public string Name { get; set; } = null!;

    public int OwnerId { get; set; }

    public bool Isdelete { get; set; }

    public virtual Owner Owner { get; set; } = null!;

    public virtual ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
}
