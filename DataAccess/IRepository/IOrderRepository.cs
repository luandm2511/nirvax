using System;
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
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<bool> CommitTransactionAsync();
        Task<bool> RollbackTransactionAsync();
        Task<bool> AddOrdersAsync(IEnumerable<Order> orders);
        Task<bool> AddOrderAsync(Order order);
        Task<IEnumerable<Order>> GetOrdersByAccountIdAsync(int accountId);
        Task<IEnumerable<Order>> GetOrdersByOwnerIdAsync(int ownerId);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<bool> UpdateOrderAsync(Order order);
        Task<IEnumerable<Order>> SearchOrdersAsync(string codeOrder);
        Task<bool> CodeOrderExistsAsync(string codeOrder);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<TopShopDTO>> GetTop10ShopsAsync();
        Task<OrderStatisticsDTO> GetOrderStatisticsAsync();
        Task<OwnerStatisticsDTO> GetOwnerStatisticsAsync(int ownerId);
    }
}
