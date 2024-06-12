using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<bool> LogAccessAsync(AccessLog accessLog)
        {
            await _context.AccessLogs.AddAsync(accessLog);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<AccessLog>> GetAccessLogsAsync()
        {
            return await _context.AccessLogs.ToListAsync();
        }

        public async Task<int> GetAccessCountAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.AccessLogs
                .CountAsync(log => log.AccessTime >= startDate && log.AccessTime <= endDate);
        }
    }
}
