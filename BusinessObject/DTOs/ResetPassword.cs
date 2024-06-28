using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class ResetPassword
    {
        public string Email { get; set; }
        [Required(ErrorMessage = "Password cannot be empty!!")]
        [MinLength(6, ErrorMessage = "Password to be at least 6 characters!!")]
        [MaxLength(24, ErrorMessage = "Password is limited to 24 characters!!")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "ConfirmPassword cannot be empty!!")]
        [MinLength(6, ErrorMessage = "ConfirmPassword to be at least 6 characters!!")]
        [MaxLength(24, ErrorMessage = "ConfirmPassword is limited to 24 characters!!")]
        public string ConfirmPassword { get; set; }
    }
}
