using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;

namespace DataAccess.Repository
{
    public class AccessLogRepository : IAccessLogRepository
    {
        private readonly AccessLogDAO _accessLogDAO;
        public AccessLogRepository(AccessLogDAO accessLogDAO)
        {
            _accessLogDAO = accessLogDAO;
        }

        public async Task<int> GetAccessCountAsync(DateTime startDate, DateTime endDate)
        {
            return await _accessLogDAO.GetAccessCountAsync(startDate, endDate);
        }

        public async Task<IEnumerable<AccessLog>> GetAccessLogsAsync()
        {
            return await _accessLogDAO.GetAccessLogsAsync();
        }

        public async Task<bool> LogAccessAsync(AccessLog accessLog)
        {
            return await _accessLogDAO.LogAccessAsync(accessLog);
        }
    }
}
