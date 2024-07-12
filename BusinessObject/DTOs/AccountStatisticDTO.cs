using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class AccountStatisticDTO
    {
        public int TotalAccount { get; set; }
        public int TotalAccountBanned { get; set; }
        public int TotalOwner { get; set;}
        public int TotalOwnerBanned { get;set;}
    }
}
