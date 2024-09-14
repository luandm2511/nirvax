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
        [Required(ErrorMessage = " ServiceId cannot be empty!!")]
        public int ServiceId { get; set; }
        [Required(ErrorMessage = " Name cannot be empty!!")]
        [MinLength(1, ErrorMessage = " Name to be at least 1 characters!!")]
        [MaxLength(50, ErrorMessage = "Name is limited to 50 characters!!")]
        public string Name { get; set; } = null!;
    }

    public class ServiceCreateDTO
    {
        [Required(ErrorMessage = " Name cannot be empty!!")]
        [MinLength(1, ErrorMessage = " Name to be at least 1 characters!!")]
        [MaxLength(50, ErrorMessage = "Name is limited to 50 characters!!")]
        public string Name { get; set; } = null!;     
    }
}
