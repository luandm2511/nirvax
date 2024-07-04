using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string Image { get; set; } = null!;

    public bool Isdelete { get; set; }

    public int CateParentId { get; set; }

    public virtual CategoryParent CateParent { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
