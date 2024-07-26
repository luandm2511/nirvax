using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTOs;
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

        public async Task<HistoryOrderDetailDTO> GetHistoryOrderDetailsByOrderIdAsync(int orderId)
        {
            var order = await _context.Orders
        .Include(o => o.Status)
        .Include(o => o.Owner)
        .Include(o => o.Voucher)
        .Include(o => o.OrderDetails)
            .ThenInclude(od => od.ProductSize)
                .ThenInclude(ps => ps.Product)
                    .ThenInclude(p => p.Images)
        .Include(o => o.OrderDetails)
            .ThenInclude(od => od.ProductSize)
                .ThenInclude(ps => ps.Size)
        .FirstOrDefaultAsync(o => o.OrderId == orderId);


            var historyOrderDetail = new HistoryOrderDetailDTO
            {
                OrderId = order.OrderId,
                CodeOrder = order.CodeOrder,
                FullName = order.Fullname,
                ShopName = order.Owner.Fullname,
                ShopImage = order.Owner.Image,
                StatusId = order.Status.StatusId,
                StatusName = order.Status.Name,
                Address = order.Address,
                Phone = order.Phone,
                OrderDate = order.OrderDate,
                RequiredDate = order.RequiredDate,
                ShippedDate = order.ShippedDate,
                VoucherId = order.VoucherId,  
                VoucherPrice = order.Voucher?.Price ?? 0,
                TotalPrice = order.TotalAmount,
                Note = order.Note,
                OrderItems = order.OrderDetails.Select(od => new HistoryOrderItemDTO
                {
                    ProductId = od.ProductSize.ProductId,
                    ProductName = od.ProductSize.Product.Name,
                    Size = od.ProductSize.Size.Name,
                    ProductImage = od.ProductSize.Product.Images.FirstOrDefault()?.LinkImage,
                    Price = od.UnitPrice,
                    Quantity = od.Quantity
                }).ToList()
            };

            return historyOrderDetail;
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            return await _context.OrderDetails
                .Include(od => od.Order)
                .Include(od => od.ProductSize)
                    .ThenInclude(ps => ps.Product)
                .Include(od => od.ProductSize)
                    .ThenInclude(ps => ps.Size)
                .Where(od => od.OrderId == orderId)
                .ToListAsync();
        }

    }
}
