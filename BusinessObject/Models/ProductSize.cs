using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.Models;

public partial class ProductSize
{
    [Key]
    [Required(ErrorMessage = " StaffID cannot be empty!!")]
    public string ProductSizeId { get; set; } = null!;
    [Required(ErrorMessage = " SizeId cannot be empty!!")]

    public int SizeId { get; set; }
    [Required(ErrorMessage = " ProductId cannot be empty!!")]

    public int ProductId { get; set; }
    [Required(ErrorMessage = " Quantity cannot be empty!!")]
    [Range(1, Int32.MaxValue, ErrorMessage = "The field quantity must be greater than {1}.")]
    public int Quantity { get; set; }

    public bool Isdelete { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Size Size { get; set; } = null!;
}
