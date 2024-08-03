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


        public string? OwnerName { get; set; }

        //public double TotalPrice { get; set; }


    }

    public class WarehouseCreateDTO
    {
        
        [Required(ErrorMessage = " OwnerId cannot be empty!!")]
        public int OwnerId { get; set; }



    }
}
