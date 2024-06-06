using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.Models;

public partial class Warehouse
{
    [Key]
    [Required(ErrorMessage = " StaffID cannot be empty!!")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int WarehouseId { get; set; }
    [Required(ErrorMessage = " OwnerId cannot be empty!!")]

    public int OwnerId { get; set; }
    [Required(ErrorMessage = " Quantity cannot be empty!!")]
    [Range(1, Int32.MaxValue, ErrorMessage = "The field totalquantity must be greater than {1}.")]
    public int TotalQuantity { get; set; }
    [Required(ErrorMessage = " Total price cannot be empty!!")]
    [Range(1, Double.MaxValue, ErrorMessage = "The field total price must be greater than {1}.")]
    public double TotalPrice { get; set; }

    public virtual ICollection<ImportProduct> ImportProducts { get; set; } = new List<ImportProduct>();

    public virtual Owner Owner { get; set; } = null!;
}
