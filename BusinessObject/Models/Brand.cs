using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Models;

public partial class Brand
{
    [Key]
    [Required(ErrorMessage = " StaffID cannot be empty!!")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int BrandId { get; set; }
    [Required(ErrorMessage = " Name cannot be empty!!")]
    [MinLength(2, ErrorMessage = " Name to be at least 2 characters!!")]
    [MaxLength(50, ErrorMessage = "Name is limited to 50 characters!!")]
    public string Name { get; set; } = null!;

    public string Image { get; set; } = null!;

    public bool Isdelete { get; set; }
    [Required(ErrorMessage = " CategoryId cannot be empty!!")]
    public int CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
