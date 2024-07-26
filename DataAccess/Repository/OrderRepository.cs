using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataAccess.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDAO _orderDAO;
        public OrderRepository(OrderDAO orderDAO)
        {
            _orderDAO = orderDAO;
        }

        public async Task AddOrderAsync(Order order)
        {
            await _orderDAO.AddOrderAsync(order);
        }

        public async Task<bool> CodeOrderExistsAsync(string codeOrder)
        {
            return await _orderDAO.CodeOrderExistsAsync(codeOrder);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderDAO.GetAllOrdersAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _orderDAO.GetOrderByIdAsync(orderId);
        }

        public async Task<IEnumerable<HistoryOrderDTO>> GetOrdersByAccountIdAsync(int accountId)
        {
            return await _orderDAO.GetOrdersByAccountIdAsync(accountId);
        }

        public async Task<IEnumerable<OrderOwnerDTO>> GetOrdersByOwnerIdAsync(int ownerId)
        {
            return await _orderDAO.GetOrdersByOwnerIdAsync(ownerId);
        }

        public async Task<OrderStatisticsDTO> GetOrderStatisticsAsync()
        {
            return await _orderDAO.GetOrderStatisticsAsync();
        }

        public async Task<OwnerStatisticsDTO> GetOwnerStatisticsAsync(int ownerId)
        {
            return await _orderDAO.GetOwnerStatisticsAsync(ownerId);
        }

        public async Task<IEnumerable<TopShopDTO>> GetTop10ShopsAsync()
        {
            return await _orderDAO.GetTop10ShopsAsync();
        }

        public async Task<IEnumerable<Order>> SearchOrdersAsync(string codeOrder)
        {
            return await _orderDAO.SearchOrdersAsync(codeOrder);
        }

        public async Task UpdateOrderAsync(Order order)
        {
            await _orderDAO.UpdateOrderAsync(order);
        }
    }
}
