using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class ItemGroup
    {
        public int OwnerId { get; set; }
        public IEnumerable<OrderItemDetailDTO> Items { get; set; }
    }
}
