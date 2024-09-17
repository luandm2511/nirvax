using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class OrderItemDetailDTO
    {
        public string ProductSizeId { get; set; }
        public string ProductName { get; set; }
        public string SizeName { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public int OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string ImageProduct { get; set; }

    }
}
