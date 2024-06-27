using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class Login
    {
        [Required(ErrorMessage = "Email cannot be empty!!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Password cannot be empty!!")]
        [MinLength(6, ErrorMessage = "Password to be at least 6 characters!!")]
        [MaxLength(24, ErrorMessage = "Password is limited to 24 characters!!")]
        public string Password { get; set; } = null!;

        public string Role { get; set; } = null!;
    }
}
