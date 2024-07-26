using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class OrderOwnerDTO
    {
        public int OrderId { get; set; }
        public string CodeOrder { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public string? Note { get; set; }
    }
}
