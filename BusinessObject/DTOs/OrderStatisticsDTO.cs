using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class OrderStatisticsDTO
    {
        public int Year { get; set; }
        public DateTime StartDate { get; set; } // Ngày bắt đầu của tuần
        public DateTime EndDate { get; set; }   // Ngày kết thúc của tuần
        public List<DailyOrderStatistics> DailyStatistics { get; set; }
    }
}
