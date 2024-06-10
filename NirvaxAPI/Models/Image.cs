using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class Image
{
    public int ImageId { get; set; }

    public string LinkImage { get; set; } = null!;

    public bool Isdelete { get; set; }

    public int ProductId { get; set; }

    public int DescriptionId { get; set; }

    public virtual Description Description { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
