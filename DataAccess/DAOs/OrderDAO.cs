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
    public class OrderDAO
    {
        private readonly NirvaxContext _context;

        public OrderDAO(NirvaxContext context)
        {
            _context = context;
        }

        public async Task<bool> AddOrders(IEnumerable<Order> orders)
        {
            await _context.Orders.AddRangeAsync(orders);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> AddOrder(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Order>> GetOrdersByAccountId(int accountId)
        {
            return await _context.Orders
                    .Include(o => o.Owner)
                    .Include(o => o.Account)
                    .Include(o => o.Status)
                    .Include(o => o.Voucher)
                    .Where(o => o.AccountId == accountId)
                    .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByOwnerId(int ownerId)
        {
            return await _context.Orders
                    .Include(o => o.Owner)
                    .Include(o => o.Account)
                    .Include(o => o.Status)
                    .Include(o => o.Voucher)
                    .Where(o => o.OwnerId == ownerId)
                    .ToListAsync();
        }

        public async Task<Order> GetOrderById(int orderId)
        {
            return await _context.Orders
                    .Include(o => o.Owner)
                    .Include(o => o.Account)
                    .Include(o => o.Status)
                    .Include(o => o.Voucher)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<bool> UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CodeOrderExists(string codeOrder)
        {
            return await _context.Orders.AnyAsync(o => o.CodeOrder == codeOrder);
        }

        public async Task<IEnumerable<Order>> SearchOrders(string codeOrder)
        {
            return await _context.Orders.Where(o => o.CodeOrder.Contains(codeOrder)).ToListAsync();
        }
    }
}
