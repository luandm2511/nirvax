using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class VoucherDTO
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

        public bool? Isdelete { get; set; }
        public bool? Isused { get; set; }
        [Required(ErrorMessage = " OwnerId cannot be empty!!")]

        public int OwnerId { get; set; }
      

    }

}
