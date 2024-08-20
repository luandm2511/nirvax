using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTOs;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAOs
{
    public class AccessLogDAO
    {
        private readonly NirvaxContext _context;

        public AccessLogDAO(NirvaxContext context)
        {
            _context = context;
        }

        public async Task LogAccessAsync(AccessLog accessLog)
        {
            await _context.AccessLogs.AddAsync(accessLog);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AccessLogDTO>> GetAccessLogsByWeekAsync()
        {
            var logs = await _context.AccessLogs.ToListAsync();

            var statistics = logs
                .GroupBy(log => new
                {
                    Year = log.AccessTime.Year,
                    StartDate = GetStartOfWeek(log.AccessTime),
                    EndDate = GetEndOfWeek(log.AccessTime),
                    DayOfWeek = (int)log.AccessTime.DayOfWeek == 0 ? 7 : (int)log.AccessTime.DayOfWeek
                })
                .Select(group => new
                {
                    Year = group.Key.Year,
                    StartDate = group.Key.StartDate,
                    EndDate = group.Key.EndDate,
                    DayOfWeek = group.Key.DayOfWeek,
                    Date = group.Min(log => log.AccessTime.Date),
                    TotalAccesses = group.Count()
                })
                .ToList();

            var result = statistics
                .GroupBy(stat => new { stat.Year, stat.StartDate, stat.EndDate })
                .Select(weekGroup => new AccessLogDTO
                {
                    Year = weekGroup.Key.Year,
                    StartDate = weekGroup.Key.StartDate,
                    EndDate = weekGroup.Key.EndDate,
                    DailyStatistics = weekGroup
                        .OrderBy(stat => stat.DayOfWeek)
                        .Select(stat => new DailyAccessLogStatistics
                        {
                            DayOfWeek = stat.DayOfWeek,
                            Date = stat.Date,
                            TotalAccesses = stat.TotalAccesses
                        })
                        .ToList()
                })
                .ToList();

            return result;
        }
        private DateTime GetStartOfWeek(DateTime date)
        {
            var diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.AddDays(-1 * diff).Date;
        }

        private DateTime GetEndOfWeek(DateTime date)
        {
            return GetStartOfWeek(date).AddDays(6);
        }


    }
}
