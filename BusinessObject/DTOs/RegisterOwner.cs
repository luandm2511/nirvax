using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class RegisterOwner
    {
        [Required(ErrorMessage = "Email cannot be empty!!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password cannot be empty!!")]
        [MinLength(6, ErrorMessage = "Password to be at least 6 characters!!")]
        [MaxLength(24, ErrorMessage = "Password is limited to 24 characters!!")]
        public string Password { get; set; }
        [Required(ErrorMessage = "ConfirmPassword cannot be empty!!")]
        [MinLength(6, ErrorMessage = "ConfirmPassword to be at least 6 characters!!")]
        [MaxLength(24, ErrorMessage = "ConfirmPassword is limited to 24 characters!!")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Name cannot be empty!!")]
        [MinLength(2, ErrorMessage = "Name to be at least 2 characters!!")]
        [MaxLength(50, ErrorMessage = "Name is limited to 50 characters!!")]
        public string Fullname { get; set; }
        [Required(ErrorMessage = "Phone cannot be empty!!")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, ErrorMessage = "Phone must be exactly characters!!")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Address cannot be empty!!")]
        [MinLength(20, ErrorMessage = "Address to be at least 20 characters!!")]
        [MaxLength(150, ErrorMessage = "Address is limited to 150 characters!!")]
        public string Address { get; set; }
    }
}
