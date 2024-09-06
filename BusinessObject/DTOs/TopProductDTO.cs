using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class TopProductDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }
        public int QuantitySold { get; set; }
        public double RatePoint { get; set; }
        public double Price { get; set; }
    }
}
