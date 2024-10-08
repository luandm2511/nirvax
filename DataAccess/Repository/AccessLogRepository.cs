﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTOs;
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


        public Task<IEnumerable<AccessLogDTO>> GetAccessLogsByWeekAsync()
        {
            return _accessLogDAO.GetAccessLogsByWeekAsync();
        }

        public async Task LogAccessAsync(AccessLog accessLog)
        {
             await _accessLogDAO.LogAccessAsync(accessLog);
        }
    }
}
