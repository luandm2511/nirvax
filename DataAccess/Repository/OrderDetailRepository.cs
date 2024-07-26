using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;

namespace DataAccess.Repository
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly OrderDetailDAO _dao;
        public OrderDetailRepository(OrderDetailDAO dao)
        {
            _dao = dao;
        }

        public async Task AddOrderDetailAsync(OrderDetail orderDetail)
        {
            await _dao.AddOrderDetailAsync(orderDetail);
        }

        public async Task<HistoryOrderDetailDTO> GetHistoryOrderDetailsByOrderIdAsync(int orderId)
        {
            return await _dao.GetHistoryOrderDetailsByOrderIdAsync(orderId);
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            return await _dao.GetOrderDetailsByOrderIdAsync(orderId);
        }
    }
}
