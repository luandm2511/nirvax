using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;

namespace DataAccess.IRepository
{
    public interface IAccessLogRepository
    {
        Task LogAccessAsync(AccessLog accessLog);
        Task<int> GetAccessLogsAsync();
    }
}
