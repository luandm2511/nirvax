using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class CategoryParent
{
    public int CateParentId { get; set; }

    public string CateParentName { get; set; } = null!;

    public bool Isdelete { get; set; }

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
