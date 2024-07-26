using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class HistoryOrderItemDTO
    {
        public int ProductId { get; set; }
        public string ProductSizeId { get; set; }
        public string ProductName { get; set; }
        public string Size { get; set; }
        public string ProductImage { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double UserRate { get; set; }
    }
}
