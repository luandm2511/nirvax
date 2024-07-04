using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class OrderDTO
    {
        public bool IsOrderNow { get; set; }
        public int AccountId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public List<VoucherOrderDTO>? Vouchers { get; set; }
        public List<OrderItemDetailDTO> Items { get; set; }
    }
}
