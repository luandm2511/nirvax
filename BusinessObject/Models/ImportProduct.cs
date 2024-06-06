using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.Models;

public partial class ImportProduct
{
    [Key]
    [Required(ErrorMessage = " StaffID cannot be empty!!")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ImportId { get; set; }
    [Required(ErrorMessage = " WarehouseId cannot be empty!!")]
    public int WarehouseId { get; set; }

    public DateTime ImportDate { get; set; }
    [Required(ErrorMessage = " Origin cannot be empty!!")]
    [MinLength(2, ErrorMessage = " Origin to be at least 2 characters!!")]
    [MaxLength(30, ErrorMessage = "Origin is limited to 30 characters!!")]
    public string Origin { get; set; } = null!;
    [Required(ErrorMessage = " Quantity cannot be empty!!")]
    [Range(1, Int32.MaxValue, ErrorMessage = "The field quantity must be greater than {1}.")]

    public int Quantity { get; set; }
    [Required(ErrorMessage = " TotalPrice cannot be empty!!")]
    [Range(1, Double.MaxValue, ErrorMessage = "The field totalprice must be greater than {1}.")]
    public double TotalPrice { get; set; }

    public virtual Warehouse Warehouse { get; set; } = null!;
}
