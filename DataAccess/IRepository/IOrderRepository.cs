﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTOs;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataAccess.IRepository
{
    public interface IOrderRepository
    {
        Task<Order> AddOrderAsync(OrderDTO createOrderDTO, ItemGroup group);
        Task<IEnumerable<HistoryOrderDTO>> GetOrdersByAccountIdAsync(int accountId);
        Task<IEnumerable<OrderOwnerDTO>> GetOrdersByOwnerIdAsync(int ownerId);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<Order> ConfirmOrder(int orderId);
        Task<Order> SucessOrder(int orderId);
        Task<Order> CancelOrder(int orderId);
        Task<Order> RejectedOrder(int orderId);
        Task<Order> FailedOrder(int orderId);
        Task<IEnumerable<Order>> SearchOrdersAsync(string codeOrder);
        Task<IEnumerable<TopShopDTO>> GetTop10ShopsAsync();
        Task<IEnumerable<OrderStatisticsDTO>> GetOrderStatisticsAsync();
        Task<IEnumerable<OrderStatisticsDTO>> GetOwnerStatisticsAsync(int ownerId);
    }
}
