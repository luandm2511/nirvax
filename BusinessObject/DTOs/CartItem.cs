using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class CartItem
    {
        public string ProductSizeId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string SizeName { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
    }
}
