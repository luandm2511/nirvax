using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class OrderStatisticsDTO
    {
        public int TotalOrders { get; set; }
        public int SuccessfulOrders { get; set; }
        public int FailedOrders { get; set; }
        public int CanceledOrders { get; set; }
        public double TotalRevenue { get; set; }
    }
}
