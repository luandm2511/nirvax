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
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDAO _orderDAO;
        public OrderRepository(OrderDAO orderDAO)
        {
            _orderDAO = orderDAO;
        }

        public async Task<bool> AddOrder(Order order)
        {
            return await _orderDAO.AddOrder(order);
        }

        public async Task<bool> AddOrders(IEnumerable<Order> orders) => await _orderDAO.AddOrders(orders);

        public async Task<bool> CodeOrderExists(string codeOrder)
        {
            return await _orderDAO.CodeOrderExists(codeOrder);
        }

        public async Task<Order> GetOrderById(int orderId)
        {
            return await _orderDAO.GetOrderById(orderId);
        }

        public async Task<IEnumerable<Order>> GetOrdersByAccountId(int accountId)
        {
            return await _orderDAO.GetOrdersByAccountId(accountId);
        }

        public async Task<IEnumerable<Order>> GetOrdersByOwnerId(int ownerId)
        {
            return await _orderDAO.GetOrdersByOwnerId(ownerId);
        }

        public async Task<IEnumerable<Order>> SearchOrders(string codeOrder)
        {
            return await _orderDAO.SearchOrders(codeOrder);
        }

        public async Task<bool> UpdateOrder(Order order)
        {
            return await _orderDAO.UpdateOrder(order);
        }
    }
}
