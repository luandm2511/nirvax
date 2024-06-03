using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class UpdateUserDTO
    {

        [Required(ErrorMessage = " FullName cannot be empty!!")]
        [MinLength(2, ErrorMessage = "FullName needs to be at least 2 characters!!")]
        [MaxLength(40, ErrorMessage = "FullName is limited to 50 characters!!")]
        public string Fullname { get; set; } = null!;

        [Required(ErrorMessage = " Phone cannot be empty!!")]
        [Phone(ErrorMessage = "Please enter the correct phone number!!")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = " Dob cannot be empty!!")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Dob { get; set; }

        [Required(ErrorMessage = " Gender cannot be empty!!")]
        public string Gender { get; set; } = null!;

        [Required(ErrorMessage = " Address cannot be empty!!")]
        [MinLength(20, ErrorMessage = "Address needs to be at least 20 characters!!")]
        [MaxLength(150, ErrorMessage = "Address is limited to 150 characters!!")]
        public string Address { get; set; } = null!;
    }
}
