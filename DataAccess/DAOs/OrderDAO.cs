using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace DataAccess.DAOs
{
    public static class OrderDAO
    {
        private static readonly NirvaxContext _context = new NirvaxContext();

        public static async Task<bool> AddOrders(IEnumerable<Order> orders)
        {
            try
            {
                await _context.Orders.AddRangeAsync(orders);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred when adding order.", ex);
            }         
        }

        public static async Task<IEnumerable<Order>> GetOrdersByAccountId(int accountId)
        {
            try
            {
                return await _context.Orders
                    .Include(o => o.Owner)
                    .Include(o => o.Account)
                    .Include(o => o.Status)
                    .Include(o => o.Voucher)
                    .Where(o => o.AccountId == accountId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving order by account {accountId}.", ex);
            }
        }

        public static async Task<IEnumerable<Order>> GetOrdersByOwnerId(int ownerId)
        {
            try
            {
                return await _context.Orders
                    .Include(o => o.Owner)
                    .Include(o => o.Account)
                    .Include(o => o.Status)
                    .Include(o => o.Voucher)
                    .Where(o => o.OwnerId == ownerId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving order by account {ownerId}.", ex);
            }
        }

        public static async Task<Order> GetOrderById(int orderId)
        {
            try
            {
                return await _context.Orders
                    .Include(o => o.Owner)
                    .Include(o => o.Account)
                    .Include(o => o.Status)
                    .Include(o => o.Voucher)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving order by account {orderId}.", ex);
            }
        }

        public static async Task<bool> UpdateOrder(Order order)
        {
            
            try
            {
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred when updating order {order.CodeOrder}.", ex);
            }
        }

        public static async Task<IEnumerable<Order>> SearchOrders(string codeOrder)
        {
            try
            {
                return await _context.Orders.Where(o => o.CodeOrder.Contains(codeOrder)).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred when updating order {codeOrder}.", ex);
            }
        }
    }
}
