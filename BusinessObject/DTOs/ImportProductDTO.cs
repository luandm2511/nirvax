using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class ImportProductDTO
    {
        [Required(ErrorMessage = "ImportId cannot be empty!!")]
        public int ImportId { get; set; }     

        public DateTime ImportDate { get; set; }
        [Required(ErrorMessage = " Origin cannot be empty!!")]
        [MinLength(2, ErrorMessage = " Origin to be at least 2 characters!!")]
        [MaxLength(30, ErrorMessage = "Origin is limited to 30 characters!!")]
        public string Origin { get; set; } = null!;
        [Required(ErrorMessage = " Quantity cannot be empty!!")]
        [Range(0, Int32.MaxValue, ErrorMessage = "The field quantity must be greater than {0}.")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = " Total price cannot be empty!!")]
        [Range(0, Double.MaxValue, ErrorMessage = "The field total price must be greater than {0}.")]
        public double TotalPrice { get; set; }
    }

    public class ImportProductCreateDTO
    {
        [Required(ErrorMessage = " OwnerId cannot be empty!!")]
        public int OwnerId { get; set; }
        public DateTime ImportDate { get; set; }
        [Required(ErrorMessage = " Origin cannot be empty!!")]
        [MinLength(2, ErrorMessage = " Origin to be at least 2 characters!!")]
        [MaxLength(30, ErrorMessage = "Origin is limited to 30 characters!!")]
        public string Origin { get; set; } = null!; 
        [Required(ErrorMessage = " Quantity cannot be empty!!")]
        [Range(0, Int32.MaxValue, ErrorMessage = "The field quantity must be greater than {0}.")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = " Total price cannot be empty!!")]
        [Range(0, Double.MaxValue, ErrorMessage = "The field total price must be greater than {0}.")]
        public double TotalPrice { get; set; }
    }
}
