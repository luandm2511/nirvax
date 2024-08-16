using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class DailyOrderStatistics
    {
        public int DayOfWeek { get; set; }
        public int TotalOrders { get; set; }
        public double TotalAmount { get; set; }
    }
}
