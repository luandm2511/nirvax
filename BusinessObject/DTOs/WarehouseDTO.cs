using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class WarehouseDTO
    {
        public int WarehouseId { get; set; }
        [Required(ErrorMessage = " OwnerId cannot be empty!!")]
        public int OwnerId { get; set; }
        // [Required(ErrorMessage = " Quantity cannot be empty!!")]
        // [Range(1, Int32.MaxValue, ErrorMessage = "The field totalquantity must be greater than {1}.")]
        //public int TotalQuantity { get; set; }

        public string? OwnerName { get; set; }

        //public double TotalPrice { get; set; }


    }

    public class WarehouseCreateDTO
    {
        
        [Required(ErrorMessage = " OwnerId cannot be empty!!")]
        public int OwnerId { get; set; }
       // [Required(ErrorMessage = " Quantity cannot be empty!!")]
        //[Range(1, Int32.MaxValue, ErrorMessage = "The field totalquantity must be greater than {1}.")]
        //public int TotalQuantity { get; set; }
       // [Required(ErrorMessage = " Total price cannot be empty!!")]
        //[Range(1, Double.MaxValue, ErrorMessage = "The field total price must be greater than {1}.")]
       // public double TotalPrice { get; set; }


    }
}
