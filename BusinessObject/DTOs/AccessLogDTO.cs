using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class AccessLogDTO
    {
        public int Year { get; set; }
        public DateTime StartDate { get; set; } // Ngày bắt đầu của tuần
        public DateTime EndDate { get; set; }   // Ngày kết thúc của tuần
        public List<DailyAccessLogStatistics> DailyStatistics { get; set; } // Thống kê theo từng ngày trong tuần
    }
}
