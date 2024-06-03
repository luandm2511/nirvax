﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;

namespace DataAccess.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        public Task<bool> AddNotificationAsync(Notification notification) => NotificationDAO.AddNotificationAsync(notification);
    }
}
