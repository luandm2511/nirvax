using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class VerifyCode
    {
        [Required(ErrorMessage = "Email cannot be empty!!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
