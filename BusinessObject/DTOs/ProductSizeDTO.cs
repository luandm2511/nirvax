using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class ProductSizeDTO
    {
        [Required(ErrorMessage = " ProductSizeId cannot be empty!!")]
        public string ProductSizeId { get; set; } = null!;
        [Required(ErrorMessage = " SizeId cannot be empty!!")]
        public int SizeId { get; set; }
        [Required(ErrorMessage = " ProductId cannot be empty!!")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = " Quantity cannot be empty!!")]
        [Range(1, Int32.MaxValue, ErrorMessage = "The field quantity must be greater than {1}.")]
        public int Quantity { get; set; }
        public string? SizeName { get; set; }
        public bool? Isdelete { get; set; }
    }
    public class ProductSizeCreateDTO
    {
        [Required(ErrorMessage = " SizeId cannot be empty!!")]
        public int SizeId { get; set; }
        [Required(ErrorMessage = " ProductId cannot be empty!!")]
        public int ProductId { get; set; }
    }

    public class ProductSizeListDTO
    {
        public string ProductSizeId { get; set; }
        public string SizeName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string ProductImage { get; set; }
        public string Status { get; set; }     
    }
}
