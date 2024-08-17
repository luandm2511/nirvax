namespace BusinessObject.DTOs
{
    public class DailyAccessLogStatistics
    {
        public int DayOfWeek { get; set; }    // Thứ trong tuần (1: Thứ 2, ..., 7: Chủ nhật)
        public DateTime Date { get; set; }    // Ngày cụ thể
        public int TotalAccesses { get; set; } // Tổng số lượt truy cập trong ngày đó
    }
}