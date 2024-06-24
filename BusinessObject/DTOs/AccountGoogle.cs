using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class AccountGoogle
    {
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Image {  get; set; }
        public string Role { get; set; }
        public bool IsBan { get; set; }
    }
}
