using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Description
{
    public int DescriptionId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public bool Isdelete { get; set; }

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
