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
using Pipelines.Sockets.Unofficial.Buffers;

namespace DataAccess.DAOs
{
    public class OrderDAO
    {
        private readonly NirvaxContext _context;

        public OrderDAO(NirvaxContext context)
        {
            _context = context;
        }

        public async Task<double> UpdateQuantityUsed(OrderDTO createOrderDTO, ItemGroup group)
        {
            var ownerVoucherDto = createOrderDTO.Vouchers?.FirstOrDefault(v => v.OwnerId == group.OwnerId);
            var voucher = new Voucher();
            if (ownerVoucherDto != null && !string.IsNullOrEmpty(ownerVoucherDto.VoucherId))
            {
                 voucher = await _context.Vouchers
                    .Where(v => v.VoucherId == ownerVoucherDto.VoucherId)
                    .FirstOrDefaultAsync();
                voucher.QuantityUsed = ++voucher.QuantityUsed;
                _context.Update(voucher);
            }
            return voucher?.Price ?? 0;
        } 
        public async Task<Order> AddOrderAsync(OrderDTO createOrderDTO, ItemGroup group, double priceVoucher)
        {
            var ownerVoucherDto = createOrderDTO.Vouchers?.FirstOrDefault(v => v.OwnerId == group.OwnerId);

            string codeOrder;
            do
            {
                codeOrder = GenerateCodeOrder();
            } while (await _context.Orders.AnyAsync(o => o.CodeOrder == codeOrder));

            var order = new Order
            {
                CodeOrder = codeOrder,
                Fullname = createOrderDTO.FullName,
                Phone = createOrderDTO.Phone,
                OrderDate = DateTime.Now,
                Address = createOrderDTO.Address,
                Note = ownerVoucherDto.Note,
                TotalAmount = group.Items.Sum(item => item.Quantity * item.UnitPrice) - priceVoucher,
                AccountId = createOrderDTO.AccountId,
                OwnerId = group.OwnerId,
                StatusId = 1,
                VoucherId = ownerVoucherDto?.VoucherId,
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }
        public async Task<IEnumerable<HistoryOrderDTO>> GetOrdersByAccountIdAsync(int accountId)
        {
            var orders = await _context.Orders
                .Include(o => o.Status)
                .Include(o => o.Owner)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ProductSize)
                        .ThenInclude(ps => ps.Product)
                            .ThenInclude(p => p.Images)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ProductSize)
                        .ThenInclude(ps => ps.Size)
                .Where(o => o.AccountId == accountId)
                .ToListAsync();

            var historyList = orders.Select(o => new HistoryOrderDTO
            {
                OrderId = o.OrderId,
                CodeOrder = o.CodeOrder,
                ShopName = o.Owner.Fullname, // Assuming Owner has a property ShopName
                ShopImage = o.Owner.Image,
                StatusName = o.Status.Name,
                ProductName = o.OrderDetails.FirstOrDefault()?.ProductSize.Product.Name,
                ProductImage = o.OrderDetails.FirstOrDefault()?.ProductSize.Product.Images.FirstOrDefault()?.LinkImage,
                Size = o.OrderDetails.FirstOrDefault()?.ProductSize.Size.Name,
                Quantity = o.OrderDetails.Sum(od => od.Quantity),
                UnitPrice = o.OrderDetails.FirstOrDefault()?.UnitPrice ?? 0,
                TotalPrice = o.TotalAmount
            }).ToList();
            return historyList;
        }

        public async Task<IEnumerable<OrderOwnerDTO>> GetOrdersByOwnerIdAsync(int ownerId)
        {
            var orders = await _context.Orders
                .Include(o => o.Owner)
                .Include(o => o.Account)
                .Include(o => o.Status)
                .Include(o => o.Voucher)
                .Include(o => o.OrderDetails) // Include OrderDetails to calculate Quantity
                .Where(o => o.OwnerId == ownerId)
                .ToListAsync();

            var orderOwnerDTOs = orders.Select(o => new OrderOwnerDTO
            {
                OrderId = o.OrderId,
                CodeOrder = o.CodeOrder,
                FullName = o.Account.Fullname,
                Address = o.Address,
                StatusId = o.Status.StatusId,
                StatusName = o.Status.Name,
                OrderDate = o.OrderDate,
                RequiredDate = o.RequiredDate,
                ShippedDate = o.ShippedDate,
                Quantity = o.OrderDetails.Sum(od => od.Quantity),
                TotalPrice = o.TotalAmount,
                Note = o.Note
            }).ToList();

            var sortedOrderOwnerDTOs = orderOwnerDTOs
                .OrderByDescending(o => new[] { o.OrderDate, o.RequiredDate ?? DateTime.MinValue, o.ShippedDate ?? DateTime.MinValue }.Max())
                .ToList();

            return sortedOrderOwnerDTOs;
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

        public async Task<IEnumerable<Order>> SearchOrdersAsync(string codeOrder)
        {
            return await _context.Orders.Where(o => o.CodeOrder.Contains(codeOrder) || o.Fullname.Contains(codeOrder)).ToListAsync();
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
            var topShops = await _context.OrderDetails
                                .Include(od => od.Order)
                                .Where(od => od.Order.StatusId == 3)
                                .GroupBy(od => new { od.Order.OwnerId, od.Order.Owner.Fullname })
                                .Select(group => new TopShopDTO
                                {
                                    OwnerName = group.Key.Fullname,
                                    TotalSalesAmount = group.Sum(od => od.Quantity * od.UnitPrice)
                                })
                                .OrderByDescending(t => t.TotalSalesAmount)
                                .Take(10).ToListAsync();
            return topShops;
        }

        private DateTime GetStartOfWeek(DateTime date)
        {
            var diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.AddDays(-1 * diff).Date;
        }

        private DateTime GetEndOfWeek(DateTime date)
        {
            return GetStartOfWeek(date).AddDays(6);
        }

        public async Task<IEnumerable<OrderStatisticsDTO>> GetOrderStatisticsAsync()
        {
            var orders = await _context.Orders.ToListAsync();

            var statistics = orders
                .GroupBy(order => new
                {
                    Year = order.OrderDate.Year,
                    StartDate = GetStartOfWeek(order.OrderDate),
                    EndDate = GetEndOfWeek(order.OrderDate),
                    DayOfWeek = (int)order.OrderDate.DayOfWeek == 0 ? 7 : (int)order.OrderDate.DayOfWeek
                })
                .Select(group => new
                {
                    Year = group.Key.Year,
                    StartDate = group.Key.StartDate,
                    EndDate = group.Key.EndDate,
                    DayOfWeek = group.Key.DayOfWeek,
                    TotalAmount = group.Sum(order => order.TotalAmount)
                })
                .ToList();

            var result = statistics
                .GroupBy(stat => new { stat.Year, stat.StartDate, stat.EndDate })
                .Select(weekGroup => new OrderStatisticsDTO
                {
                    Year = weekGroup.Key.Year,
                    StartDate = weekGroup.Key.StartDate,
                    EndDate = weekGroup.Key.EndDate,
                    DailyStatistics = weekGroup
                        .OrderBy(stat => stat.DayOfWeek)
                        .Select(stat => new DailyOrderStatistics
                        {
                            DayOfWeek = stat.DayOfWeek,
                            TotalAmount = stat.TotalAmount
                        })
                        .ToList()
                })
                .ToList();

            return result;

        }

        public async Task<IEnumerable<OrderStatisticsDTO>> GetOwnerStatisticsAsync(int ownerId)
        {
            var orders = await _context.Orders.Where(o => o.OwnerId == ownerId).ToListAsync();

            var statistics = orders
                .GroupBy(order => new
                {
                    Year = order.OrderDate.Year,
                    StartDate = GetStartOfWeek(order.OrderDate),
                    EndDate = GetEndOfWeek(order.OrderDate),
                    DayOfWeek = (int)order.OrderDate.DayOfWeek == 0 ? 7 : (int)order.OrderDate.DayOfWeek
                })
                .Select(group => new
                {
                    Year = group.Key.Year,
                    StartDate = group.Key.StartDate,
                    EndDate = group.Key.EndDate,
                    DayOfWeek = group.Key.DayOfWeek,
                    TotalOrders = group.Count(),
                    TotalAmount = group.Sum(order => order.TotalAmount)
                })
                .ToList();

            var result = statistics
                .GroupBy(stat => new { stat.Year, stat.StartDate, stat.EndDate })
                .Select(weekGroup => new OrderStatisticsDTO
                {
                    Year = weekGroup.Key.Year,
                    StartDate = weekGroup.Key.StartDate,
                    EndDate = weekGroup.Key.EndDate,
                    DailyStatistics = weekGroup
                        .OrderBy(stat => stat.DayOfWeek)
                        .Select(stat => new DailyOrderStatistics
                        {
                            DayOfWeek = stat.DayOfWeek,
                            TotalAmount = stat.TotalAmount
                        })
                        .ToList()
                })
                .ToList();
            return result;
        }
        private string GenerateCodeOrder()
        {
            var random = new Random();
            return new string(Enumerable.Repeat("0123456789", 10).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
