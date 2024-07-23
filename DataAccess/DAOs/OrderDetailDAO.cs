using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAOs
{
    public class OrderDetailDAO
    {
        private readonly NirvaxContext _context;

        public OrderDetailDAO(NirvaxContext context)
        {
            _context = context;
        }

        public async Task AddOrderDetailAsync(OrderDetail orderDetail)
        {
            _context.OrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            return await _context.OrderDetails
                .Include(o => o.Order)
                .Include(o => o.ProductSize)
                    .ThenInclude(ps => ps.Product)
                .Include(o => o.ProductSize)
                    .ThenInclude(ps => ps.Size)
                .Where(od => od.OrderId == orderId)
                .ToListAsync();
        }
    }
}
