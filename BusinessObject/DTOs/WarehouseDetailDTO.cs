using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class WarehouseDetailDTO
    {
        public int WarehouseId { get; set; }
        [Required(ErrorMessage = " ProductSizeId cannot be empty!!")]

        public string ProductSizeId { get; set; } = null!;
        [Required(ErrorMessage = " Quantity cannot be empty!!")]
        [Range(1, Int32.MaxValue, ErrorMessage = "The field quantity must be greater than {1}.")]
        public int QuantityInStock { get; set; }
        [Required(ErrorMessage = " Location cannot be empty!!")]
        [MinLength(2, ErrorMessage = " Location to be at least 2 characters!!")]
        [MaxLength(50, ErrorMessage = "Location is limited to 50 characters!!")]
        public string Location { get; set; } = null!;
        [Required(ErrorMessage = " Unit price cannot be empty!!")]
        [Range(1, Double.MaxValue, ErrorMessage = "The field unit price must be greater than {1}.")]
        public double UnitPrice { get; set; }

    }

    public class WarehouseDetailFinalDTO
    {

        public int WarehouseId { get; set; }
        [Required(ErrorMessage = " ProductSizeId cannot be empty!!")]

        public string ProductSizeId { get; set; } = null!;
        [Required(ErrorMessage = " Quantity cannot be empty!!")]
        [Range(1, Int32.MaxValue, ErrorMessage = "The field quantity must be greater than {1}.")]
        public int TotalQuantity { get; set; }
        [Required(ErrorMessage = " Location cannot be empty!!")]
        [MinLength(2, ErrorMessage = " Location to be at least 2 characters!!")]
        [MaxLength(50, ErrorMessage = "Location is limited to 50 characters!!")]
        public string Location { get; set; } = null!;
        [Required(ErrorMessage = " Unit price cannot be empty!!")]
        [Range(1, Double.MaxValue, ErrorMessage = "The field unit price must be greater than {1}.")]
        public double TotalUnitPrice { get; set; }

    }
}
