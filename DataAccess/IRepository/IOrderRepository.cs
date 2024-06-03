using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;

namespace DataAccess.IRepository
{
    public interface IOrderRepository
    {
        Task<bool> AddOrders(IEnumerable<Order> orders);
        Task<IEnumerable<Order>> GetOrdersByAccountId(int accountId);
        Task<IEnumerable<Order>> GetOrdersByOwnerId(int ownerId);
        Task<Order> GetOrderById(int orderId);
        Task<bool> UpdateOrder(Order order);
        Task<IEnumerable<Order>> SearchOrders(string codeOrder);
    }
}
