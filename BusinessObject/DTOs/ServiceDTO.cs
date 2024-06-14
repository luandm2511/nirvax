using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class ServiceDTO
    {
        public int ServiceId { get; set; }
        [Required(ErrorMessage = " Name cannot be empty!!")]
        [MinLength(2, ErrorMessage = " Name to be at least 2 characters!!")]
        [MaxLength(50, ErrorMessage = "Name is limited to 50 characters!!")]
        public string Name { get; set; } = null!;

        public bool? Isdelete { get; set; }
    }

    public class ServiceCreateDTO
    {
  
        [Required(ErrorMessage = " Name cannot be empty!!")]
        [MinLength(2, ErrorMessage = " Name to be at least 2 characters!!")]
        [MaxLength(50, ErrorMessage = "Name is limited to 50 characters!!")]
        public string Name { get; set; } = null!;

        public bool? Isdelete { get; set; }
    }
}
