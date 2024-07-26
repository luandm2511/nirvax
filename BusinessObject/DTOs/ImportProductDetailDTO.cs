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
       // public int ImportId { get; set; }
        [Required(ErrorMessage = " ProductSizeId received cannot be empty!!")]

        public string ProductSizeId { get; set; } = null!;
        [Required(ErrorMessage = " Quantity received cannot be empty!!")]
        [Range(1, Int32.MaxValue, ErrorMessage = "The field quantity received must be greater than {1}.")]
        public int QuantityReceived { get; set; }
        [Required(ErrorMessage = " Unit price cannot be empty!!")]
        [Range(1, Double.MaxValue, ErrorMessage = "The field unit price must be greater than {1}.")]
       public double UnitPrice { get; set; }
    }
}
