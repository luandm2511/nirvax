using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class CartOwner
    {
        public int OwnerId { get; set; }
        public string OwnerName { get; set; }
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
