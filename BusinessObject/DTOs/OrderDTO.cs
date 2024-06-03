using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class OrderDTO
    {
        public DateTime RequiredDate { get; set; }
        public int AccountId { get; set; }
        public List<CartItemDTO> CartItems { get; set; }
        public string? VoucherId { get; set; }
    }
}
