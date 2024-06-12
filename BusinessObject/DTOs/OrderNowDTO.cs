using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class OrderNowDTO
    {
        public int AccountId { get; set; }
        public string ProductSizeId { get; set; }
        public int Quantity { get; set; }
        public DateTime RequiredDate { get; set; }
        public string? VoucherId { get; set; }
    }
}
