using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class ImportProductDetailDTO
    {
        public int ImportId { get; set; }
        [Required(ErrorMessage = " ProductSizeId received cannot be empty!!")]

        public string ProductSizeId { get; set; } = null!;
        [Required(ErrorMessage = " Quantity received cannot be empty!!")]
        [Range(1, Int32.MaxValue, ErrorMessage = "The field quantity received must be greater than {1}.")]
        public int QuantityReceived { get; set; }
        [Required(ErrorMessage = " Unit price cannot be empty!!")]
        [Range(1, Double.MaxValue, ErrorMessage = "The field unit price must be greater than {1}.")]
        public double UnitPrice { get; set; }
    }

    public class ImportProductDetailForImportDTO
    {
        public int ImportId { get; set; }
        [Required(ErrorMessage = " ProductSizeId received cannot be empty!!")]
        public string ProductSizeId { get; set; } = null!;
        public int TotalQuantity { get; set; }
        public double TotalPrice { get; set; }
    }

    public class ImportProductDetailCreateDTO
    {
        public int ProductId { get; set; }
        public int SizeId { get; set; }

        [Required(ErrorMessage = " Quantity received cannot be empty!!")]
        [Range(1, Int32.MaxValue, ErrorMessage = "The field quantity received must be greater than {1}.")]
        public int QuantityReceived { get; set; }
        [Required(ErrorMessage = " Unit price cannot be empty!!")]
        [Range(1, Double.MaxValue, ErrorMessage = "The field unit price must be greater than {1}.")]
        public double UnitPrice { get; set; }
    }

    public class ImportProductDetailUpdateDTO
    {
        public int SizeId { get; set; } 
        public int ImportId { get; set; }
        public string ProductSizeId { get; set; }

        [Required(ErrorMessage = " Quantity received cannot be empty!!")]
        [Range(1, Int32.MaxValue, ErrorMessage = "The field quantity received must be greater than {1}.")]
        public int QuantityReceived { get; set; }
        [Required(ErrorMessage = " Unit price cannot be empty!!")]
        [Range(1, Double.MaxValue, ErrorMessage = "The field unit price must be greater than {1}.")]
        public double UnitPrice { get; set; }
    }

    public class ImportProductDetailByImportDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public int SizeId { get; set; }
        public string SizeName { get; set; }

        public int ImportId { get; set; }
        public int QuantityReceived { get; set; }
        public double UnitPrice { get; set; }
    }
}
