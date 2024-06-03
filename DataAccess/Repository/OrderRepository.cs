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
        public async Task<bool> AddOrders(IEnumerable<Order> orders) => await OrderDAO.AddOrders(orders);
        public async Task<Order> GetOrderById(int orderId)
        {
            return await OrderDAO.GetOrderById(orderId);
        }

        public async Task<IEnumerable<Order>> GetOrdersByAccountId(int accountId)
        {
            return await OrderDAO.GetOrdersByAccountId(accountId);
        }

        public async Task<IEnumerable<Order>> GetOrdersByOwnerId(int ownerId)
        {
            return await OrderDAO.GetOrdersByOwnerId(ownerId);
        }

        public async Task<IEnumerable<Order>> SearchOrders(string codeOrder)
        {
            return await OrderDAO.SearchOrders(codeOrder);
        }

        public async Task<bool> UpdateOrder(Order order)
        {
            return await OrderDAO.UpdateOrder(order);
        }
    }
}
