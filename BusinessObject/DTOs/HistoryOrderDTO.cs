using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class HistoryOrderDTO
    {
        public int OrderId { get; set; }
        public string ShopName { get; set; }
        public string StatusName { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string Size { get; set; }
        public double UnitPrice { get; set; }
        public double TotalPrice { get; set; }
    }
}
