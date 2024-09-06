using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class AccountStatisticDTO
    {
        public int TotalAccountUser { get; set; }
        public int TotalAccountOwner { get; set;}
        public List<AccountStatisticByTime> AccountStatistics { get; set; } = new List<AccountStatisticByTime>();
    }
}
