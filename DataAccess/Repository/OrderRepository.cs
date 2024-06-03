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
        public Task<bool> AddOrders(IEnumerable<Order> orders) =>  OrderDAO.AddOrders(orders);
        public Task<Order> GetOrderById(int orderId)
        {
            return OrderDAO.GetOrderById(orderId);
        }

        public Task<IEnumerable<Order>> GetOrdersByAccountId(int accountId)
        {
            return OrderDAO.GetOrdersByAccountId(accountId);
        }

        public Task<IEnumerable<Order>> GetOrdersByOwnerId(int ownerId)
        {
            return OrderDAO.GetOrdersByOwnerId(ownerId);
        }

        public Task<IEnumerable<Order>> SearchOrders(string codeOrder)
        {
            return OrderDAO.SearchOrders(codeOrder);
        }

        public Task<bool> UpdateOrder(Order order)
        {
            return OrderDAO.UpdateOrder(order);
        }
    }
}
