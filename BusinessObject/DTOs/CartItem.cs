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
        public string ProductName { get; set; }
        public string Size { get; set; }
        public double Price { get; set; }
        public double TotalPrice { get; set; }
        public int Quantity { get; set; }
        public string Image {  get; set; }
        public int OwnerId { get; set; }
    }
}
