using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTOs;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Identity.Client;

namespace DataAccess.DAOs
{
    public class OrderDAO
    {
        private readonly NirvaxContext _context;
        private IDbContextTransaction _transaction;

        public OrderDAO(NirvaxContext context)
        {
            _context = context;
        }
        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Order>> GetOrdersByAccountIdAsync(int accountId)
        {
            return await _context.Orders
                    .Include(o => o.Owner)
                    .Include(o => o.Status)
                    .Include(o => o.Voucher)
                    .AsNoTracking()
                    .Where(o => o.AccountId == accountId)
                    .OrderByDescending(o => o.OrderId)
                    .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByOwnerIdAsync(int ownerId)
        {
            return await _context.Orders
                    .Include(o => o.Owner)
                    .Include(o => o.Account)
                    .Include(o => o.Status)
                    .Include(o => o.Voucher)
                    .Where(o => o.OwnerId == ownerId)
                    .OrderByDescending(o => o.OrderId)
                    .ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                    .Include(o => o.Owner)
                    .Include(o => o.Account)
                    .Include(o => o.Status)
                    .Include(o => o.Voucher)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CodeOrderExistsAsync(string codeOrder)
        {
            return await _context.Orders.AnyAsync(o => o.CodeOrder == codeOrder);
        }

        public async Task<IEnumerable<Order>> SearchOrdersAsync(string codeOrder)
        {
            return await _context.Orders.Where(o => o.CodeOrder.Contains(codeOrder)).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.Include(o => o.Owner)
                    .Include(o => o.Account)
                    .Include(o => o.Status)
                    .Include(o => o.Voucher)
                    .Include(o => o.OrderDetails).ToListAsync();
        }

        public async Task<IEnumerable<TopShopDTO>> GetTop10ShopsAsync()
        {
            var topShops = await _context.Orders
                .GroupBy(o => o.OwnerId)
                .Select(group => new TopShopDTO
                {
                    OwnerId = group.Key,
                    TotalProductsSold = group.Sum(o => o.OrderDetails.Sum(od => od.Quantity))
                })
                .OrderByDescending(t => t.TotalProductsSold)
                .Take(10)
                .ToListAsync();

            return topShops;
        } 
        public async Task<OrderStatisticsDTO> GetOrderStatisticsAsync()
        {
            var totalOrders = await _context.Orders.CountAsync();
            var successfulOrders = await _context.Orders.CountAsync(o => o.StatusId == 3); // Assuming 2 is for successful status
            var failedOrders = await _context.Orders.CountAsync(o => o.StatusId == 4); // Assuming 3 is for failed status
            var canceledOrders = await _context.Orders.CountAsync(o => o.StatusId == 5); // Assuming 4 is for canceled status

            var totalRevenue = await _context.Orders.SumAsync(o => o.TotalAmount);

            return new OrderStatisticsDTO
            {
                TotalOrders = totalOrders,
                SuccessfulOrders = successfulOrders,
                FailedOrders = failedOrders,
                CanceledOrders = canceledOrders,
                TotalRevenue = totalRevenue
            };
        }

        public async Task<OwnerStatisticsDTO> GetOwnerStatisticsAsync(int ownerId)
        {
            var totalOrders = await _context.Orders.CountAsync(o => o.OwnerId == ownerId);
            var successfulOrders = await _context.Orders.CountAsync(o => o.OwnerId == ownerId && o.StatusId == 3);
            var failedOrders = await _context.Orders.CountAsync(o => o.OwnerId == ownerId && o.StatusId == 4);
            var canceledOrders = await _context.Orders.CountAsync(o => o.OwnerId == ownerId && o.StatusId == 5);

            var totalRevenue = await _context.Orders
                .Where(o => o.OwnerId == ownerId)
                .SumAsync(o => o.TotalAmount);

            return new OwnerStatisticsDTO
            {
                OwnerId = ownerId,
                TotalOrders = totalOrders,
                SuccessfulOrders = successfulOrders,
                FailedOrders = failedOrders,
                CanceledOrders = canceledOrders,
                TotalRevenue = totalRevenue
            };
        }
    }
}
