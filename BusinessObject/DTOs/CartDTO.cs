using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class CartDTO
    {
        public int TotalCount { get; set; }
        public List<CartOwner> CartOwners { get; set; } = new List<CartOwner>();
    }
}
