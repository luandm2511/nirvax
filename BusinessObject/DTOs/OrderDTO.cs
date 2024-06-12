using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class OrderDTO
    {
        public int AccountId { get; set; }
        public DateTime RequiredDate { get; set; }
        public List<string> CartItemIds { get; set; } = new List<string>();
        public string? VoucherId { get; set; }
    }
}
