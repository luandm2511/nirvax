using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class AccountGoogle
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
