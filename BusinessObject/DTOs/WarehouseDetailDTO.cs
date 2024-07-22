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

        [Required(ErrorMessage = " Location cannot be empty!!")]
        [MinLength(2, ErrorMessage = " Location to be at least 2 characters!!")]
        [MaxLength(50, ErrorMessage = "Location is limited to 50 characters!!")]
        public string Location { get; set; } = null!;
       
        

    }

    public class WarehouseDetailFinalDTO
    {

        public int WarehouseId { get; set; }
        [Required(ErrorMessage = " ProductSizeId cannot be empty!!")]

        public string ProductSizeId { get; set; } = null!;

        [Required(ErrorMessage = " Location cannot be empty!!")]
        [MinLength(2, ErrorMessage = " Location to be at least 2 characters!!")]
        [MaxLength(50, ErrorMessage = "Location is limited to 50 characters!!")]
        public string Location { get; set; } = null!;
  
   

    }
}
