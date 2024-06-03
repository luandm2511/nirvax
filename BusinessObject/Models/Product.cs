using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public string ShortDescription { get; set; } = null!;

    public double Price { get; set; }

    public int QuantitySold { get; set; }

    public double RatePoint { get; set; }

    public int RateCount { get; set; }

    public bool Isdelete { get; set; }

    public bool Isban { get; set; }

    public int DescriptionId { get; set; }

    public int CategoryId { get; set; }

    public int? BrandId { get; set; }

    public int OwnerId { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Description Description { get; set; } = null!;

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual Owner Owner { get; set; } = null!;

    public virtual ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
}
