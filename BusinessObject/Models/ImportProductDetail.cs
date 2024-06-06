using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.Models;

public partial class ImportProductDetail
{
    //của importProduct chứ kh phải của detai;
    public int ImportId { get; set; }
    [Required(ErrorMessage = " ProductSizeId cannot be empty!!")]

    public string ProductSizeId { get; set; } = null!;
    [Required(ErrorMessage = " Quantity cannot be empty!!")]
    [Range(1, Int32.MaxValue, ErrorMessage = "The field quantity must be greater than {1}.")]

    public int QuantityReceived { get; set; }
    [Required(ErrorMessage = " UnitPrice cannot be empty!!")]
    [Range(1, Double.MaxValue, ErrorMessage = "The field unit price must be greater than {1}.")]
    public double UnitPrice { get; set; }

    public virtual ImportProduct Import { get; set; } = null!;

    public virtual ProductSize ProductSize { get; set; } = null!;
}
