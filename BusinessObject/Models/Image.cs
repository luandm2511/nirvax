using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Image
{
    public int ImageId { get; set; }

    public string LinkImage { get; set; } = null!;

    public bool Isdelete { get; set; }

    public int? ProductId { get; set; }

    public int? SizeChartId { get; set; }

    public virtual Product? Product { get; set; }

    public virtual SizeChart? SizeChart { get; set; }
}
