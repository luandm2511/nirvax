using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataAccess.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDAO _orderDAO;
        private readonly ProductDAO _productDAO;
        private readonly OrderDetailDAO _orderDetailDAO;
        private readonly ProductSizeDAO _productSizeDAO;
        public OrderRepository(OrderDAO orderDAO, ProductDAO productDAO, OrderDetailDAO orderDetailDAO, ProductSizeDAO productSizeDAO)
        {
            _orderDAO = orderDAO;
            _productDAO = productDAO;
            _orderDetailDAO = orderDetailDAO;
            _productSizeDAO = productSizeDAO;
        }

        public async Task<Order> AddOrderAsync(OrderDTO createOrderDTO, ItemGroup group)
        {
            var priceVoucher = await _orderDAO.UpdateQuantityUsed(createOrderDTO, group);
            var order = await _orderDAO.AddOrderAsync(createOrderDTO, group, priceVoucher);
            return order;
        }

        public async Task<Order> ConfirmOrder(int orderId)
        {
            var orderDetails = await _orderDetailDAO.GetOrderDetailsByOrderIdAsync(orderId);
            foreach (var detail in orderDetails)
            {
                var productSize = await _productSizeDAO.GetByIdAsync(detail.ProductSizeId);
                productSize.Quantity -= detail.Quantity;
                await _productSizeDAO.UpdateAsync(productSize);
            }
            var order = await _orderDAO.GetOrderByIdAsync(orderId);
            order.StatusId = 2;
            order.RequiredDate = DateTime.Now;
            await _orderDAO.UpdateOrderAsync(order);
            return order;
        }

        public async Task<Order> CancelOrder(int orderId)
        { 
            var order = await _orderDAO.GetOrderByIdAsync(orderId);
            order.StatusId = 5;
            order.RequiredDate = DateTime.Now;
            await _orderDAO.UpdateOrderAsync(order);
            return order;
        }
        public async Task<Order> RejectedOrder(int orderId)
        {
            var order = await _orderDAO.GetOrderByIdAsync(orderId);
            order.StatusId = 4;
            order.RequiredDate = DateTime.Now;
            await _orderDAO.UpdateOrderAsync(order);
            return order;
        }
        public async Task<Order> FailedOrder(int orderId)
        {
            var orderDetails = await _orderDetailDAO.GetOrderDetailsByOrderIdAsync(orderId);
            foreach (var detail in orderDetails)
            {
                var productSize = await _productSizeDAO.GetByIdAsync(detail.ProductSizeId);
                productSize.Quantity += detail.Quantity;
                await _productSizeDAO.UpdateAsync(productSize);
            }
            var order = await _orderDAO.GetOrderByIdAsync(orderId);
            order.StatusId = 6;
            order.ShippedDate = DateTime.Now;
            await _orderDAO.UpdateOrderAsync(order);
            return order;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderDAO.GetAllOrdersAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _orderDAO.GetOrderByIdAsync(orderId);
        }

        public async Task<IEnumerable<HistoryOrderDTO>> GetOrdersByAccountIdAsync(int accountId)
        {
            return await _orderDAO.GetOrdersByAccountIdAsync(accountId);
        }

        public async Task<IEnumerable<OrderOwnerDTO>> GetOrdersByOwnerIdAsync(int ownerId)
        {
            return await _orderDAO.GetOrdersByOwnerIdAsync(ownerId);
        }

        public async Task<IEnumerable<OrderStatisticsDTO>> GetOrderStatisticsAsync()
        {
            return await _orderDAO.GetOrderStatisticsAsync();
        }

        public async Task<IEnumerable<OrderStatisticsDTO>> GetOwnerStatisticsAsync(int ownerId)
        {
            return await _orderDAO.GetOwnerStatisticsAsync(ownerId);
        }

        public async Task<IEnumerable<TopShopDTO>> GetTop10ShopsAsync()
        {
            return await _orderDAO.GetTop10ShopsAsync();
        }

        public async Task<IEnumerable<Order>> SearchOrdersAsync(string codeOrder)
        {
            return await _orderDAO.SearchOrdersAsync(codeOrder);
        }

        public async Task<Order> SucessOrder(int orderId)
        {
            var orderDetail = await _orderDetailDAO.GetOrderDetailsByOrderIdAsync(orderId);
            foreach (var detail in orderDetail)
            {
                var product = await _productDAO.GetByIdAsync(detail.ProductSize.ProductId);
                    product.QuantitySold += detail.Quantity;
                    await _productDAO.UpdateAsync(product);
            }
            var order = await _orderDAO.GetOrderByIdAsync(orderId);
            order.ShippedDate = DateTime.Now;
            order.StatusId = 3;
            await _orderDAO.UpdateOrderAsync(order);
            return order;
        }

    }
}
