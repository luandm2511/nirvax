using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class VoucherOrderDTO
    {
        public int? OwnerId { get; set; }
        public string? VoucherId { get; set; }
        public string? Note { get; set; }
    }
}
