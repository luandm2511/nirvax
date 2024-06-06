using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.Models;

public partial class Voucher
{
    public string VoucherId { get; set; } = null!;
    [Required(ErrorMessage = " TotalPrice cannot be empty!!")]
    [Range(1, Double.MaxValue, ErrorMessage = "The field price must be greater than {1}.")]
    public double Price { get; set; }
    [Required(ErrorMessage = " Quantity cannot be empty!!")]
    [Range(1, Int32.MaxValue, ErrorMessage = "The field quantity must be greater than {1}.")]
    public int Quantity { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
    [Required(ErrorMessage = " OwnerId cannot be empty!!")]

    public int OwnerId { get; set; }

    public bool Isdelete { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Owner Owner { get; set; } = null!;
}
