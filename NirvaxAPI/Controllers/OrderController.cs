using System.Linq;
using AutoMapper;
using Azure.Core;
using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Helpers;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IProductSizeRepository _productSizeRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IVoucherRepository _voucherRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderController(
            IOrderRepository orderRepository,
            IOrderDetailRepository orderDetailRepository,
            IProductSizeRepository productSizeRepository,
            INotificationRepository notificationRepository,
            IVoucherRepository voucherRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _productSizeRepository = productSizeRepository;
            _notificationRepository = notificationRepository;
            _voucherRepository = voucherRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("get-order-items")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetOrderItems([FromBody] List<OrderItemDTO> orderItems)
        {
            var orderItemDetails = new List<OrderItemDetailDTO>();

            foreach (var item in orderItems)
            {
                var productSize = await _productSizeRepository.GetByIdAsync(item.ProductSizeId);

                if (productSize != null && !productSize.Isdelete && !productSize.Product.Isdelete && !productSize.Product.Isban && productSize.Quantity >= item.Quantity)
                {
                    orderItemDetails.Add(new OrderItemDetailDTO
                    {
                        ProductSizeId = productSize.ProductSizeId,
                        ProductName = productSize.Product.Name,
                        SizeName = productSize.Size.Name,
                        Quantity = item.Quantity,
                        UnitPrice = productSize.Product.Price,
                        OwnerId = productSize.Product.OwnerId
                    });
                }
                else
                {
                    return BadRequest($"The {productSize.Product.Name} you purchased is not found or the quantity in stock is not enough for the product you want to order.");
                }
            }

            return Ok(orderItemDetails);
        }

        [HttpPost("check-voucher")]
        public async Task<IActionResult> CheckVoucher([FromBody] VoucherOrderDTO request)
        {
            var voucher = await _voucherRepository.GetVoucherById(request.VoucherId);

            if (voucher == null || voucher.OwnerId != request.OwnerId || voucher.EndDate > DateTime.UtcNow)
            {
                return BadRequest("Invalid voucher");
            }

            return Ok();
        }

        [HttpPost]
        //[Authorize(Roles = "User")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDTO createOrderDTO)
        {
            using var transaction = await _orderRepository.BeginTransactionAsync();
            try
            {
                var cart = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<CartOwner>>($"Cart_{createOrderDTO.AccountId}") ?? new List<CartOwner>();

                // Group các sản phẩm theo OwnerId
                var groupedItems = createOrderDTO.Items.GroupBy(item => item.OwnerId);

                // Validate vouchers
                if (createOrderDTO.Vouchers != null && createOrderDTO.Vouchers.Any())
                {
                    foreach (var voucherDto in createOrderDTO.Vouchers)
                    {
                        var voucher = await _voucherRepository.GetVoucherById(voucherDto.VoucherId);
                        if (voucher == null || voucher.OwnerId != voucherDto.OwnerId || voucher.EndDate < DateTime.UtcNow)
                        {
                            return BadRequest($"{voucherDto.OwnerId} is invalid.");
                        }
                    }
                }

                var orders = new List<Order>();

                foreach (var group in groupedItems)
                {
                    var ownerVoucher = createOrderDTO.Vouchers.FirstOrDefault(v => v.OwnerId == group.Key)?.VoucherId;

                    var voucher = await _voucherRepository.GetVoucherById(ownerVoucher);

                    string codeOrder;
                    do
                    {
                        codeOrder = GenerateCodeOrder();
                    } while (await _orderRepository.CodeOrderExistsAsync(codeOrder));

                    var order = new Order
                    {
                        CodeOrder = codeOrder,
                        Fullname = createOrderDTO.FullName,
                        Phone = createOrderDTO.Phone,
                        OrderDate = DateTime.UtcNow,
                        Address = createOrderDTO.Address,
                        Note = createOrderDTO.Note,
                        TotalAmount = group.Sum(item => item.Quantity * item.UnitPrice) - voucher.Price,
                        AccountId = createOrderDTO.AccountId,
                        OwnerId = group.Key,
                        StatusId = 1,
                        VoucherId = ownerVoucher
                    };

                    await _orderRepository.AddOrderAsync(order);

                    await _voucherRepository.PriceAndQuantityByOrderAsync(voucher.VoucherId);

                    foreach (var cartItem in group)
                    {
                        var productSize = await _productSizeRepository.GetByIdAsync(cartItem.ProductSizeId);
                        if (productSize != null && productSize.Quantity >= cartItem.Quantity)
                        {
                            productSize.Quantity -= cartItem.Quantity;
                            await _productSizeRepository.UpdateAsync(productSize);

                            // Add order details
                            var orderDetail = new OrderDetail
                            {
                                ProductSizeId = cartItem.ProductSizeId,
                                Quantity = cartItem.Quantity,
                                UnitPrice = cartItem.UnitPrice,
                                OrderId = order.OrderId
                            };
                            await _orderDetailRepository.AddOrderDetailAsync(orderDetail);
                            productSize.Quantity -= cartItem.Quantity;
                            await _productSizeRepository.UpdateAsync(productSize);
                            // Remove item from session cart
                            if (!createOrderDTO.IsOrderNow)
                            {
                                RemoveCartItemFromSession(createOrderDTO.AccountId, cartItem.ProductSizeId);
                            }
                        }
                        else
                        {
                            return BadRequest($"Insufficient quantity for ProductSizeId {cartItem.ProductSizeId}");
                        }
                    }

                    var notification = new Notification
                    {
                        AccountId = null,
                        OwnerId = group.Key, // Assuming Product model has OwnerId field
                        Content = $"You has just had a order with code order {codeOrder}.",
                        IsRead = false,
                        Url = null,
                        CreateDate = DateTime.UtcNow
                    };
                     await _notificationRepository.AddNotificationAsync(notification);
                }

                await _orderRepository.CommitTransactionAsync();

                return Ok(new { Orders = _mapper.Map<IEnumerable<OrderDTO>>(orders) });
            }
            catch (Exception ex)
            {
                await _orderRepository.RollbackTransactionAsync();
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("history/{accountId}")]
        //[Authorize(Roles = "User")]
        public async Task<IActionResult> ViewOrderHistory(int accountId)
        {
            try
            {
                var orders = await _orderRepository.GetOrdersByAccountIdAsync(accountId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("owner/{ownerId}")]
        [Authorize(Roles = "Owner,Staff")]
        public async Task<IActionResult> ViewOrdersByOwner(int ownerId)
        {
            try
            {
                var orders = await _orderRepository.GetOrdersByOwnerIdAsync(ownerId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("confirm/{orderId}/{statusId}")]

        public async Task<IActionResult> ConfirmOrder(int orderId, int statusId)
        {
            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(orderId);
                if (order == null)
                {
                    return NotFound();
                }

                if(statusId == 2)
                {
                    order.RequiredDate = DateTime.UtcNow;
                }

                if (statusId > 3) // Assuming > 3 means order is canceled or returned
                {
                    var orderDetail = await _orderDetailRepository.GetOrderDetailsByOrderIdAsync(orderId);
                    foreach (var detail in orderDetail)
                    {
                        var productSize = await _productSizeRepository.GetByIdAsync(detail.ProductSizeId);
                        if (productSize != null)
                        {
                            productSize.Quantity += detail.Quantity;
                            await _productSizeRepository.UpdateAsync(productSize);
                        }
                    }
                    order.ShippedDate = DateTime.UtcNow;
                }

                order.StatusId = statusId; 
                await _orderRepository.UpdateOrderAsync(order);

                // Notify user about order confirmation (implementation skipped)
                var notification = new Notification
                {
                    AccountId = order.AccountId,
                    OwnerId = null, // Assuming Product model has OwnerId field
                    Content = $"You have an order with order code {order.CodeOrder} being in {order.Status.Name} status  .",
                    IsRead = false,
                    Url = null,
                    CreateDate = DateTime.UtcNow
                };

                await _notificationRepository.AddNotificationAsync(notification);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("cancel/{orderId}")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(orderId);
                if (order == null)
                {
                    return NotFound();
                }

                var orderDetails = await _orderDetailRepository.GetOrderDetailsByOrderIdAsync(orderId);
                foreach (var detail in orderDetails)
                {
                    var productSize = await _productSizeRepository.GetByIdAsync(detail.ProductSizeId);
                    if (productSize != null)
                    {
                        productSize.Quantity += detail.Quantity;
                        await _productSizeRepository.UpdateAsync(productSize);
                    }
                }

                order.StatusId = 5;
                await _orderRepository.UpdateOrderAsync(order);

                var notification = new Notification
                {
                    AccountId = null,
                    OwnerId = order.OwnerId, 
                    Content = $"The order with order code {order.CodeOrder} has been canceled by user.",
                    IsRead = false,
                    Url = null,
                    CreateDate = DateTime.UtcNow
                };

                await _notificationRepository.AddNotificationAsync(notification);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("search")]
        [Authorize(Roles = "Owner,Staff")]
        public async Task<IActionResult> SearchOrders([FromQuery] string codeOrder)
        {
            try
            {
                var orders = await _orderRepository.SearchOrdersAsync(codeOrder);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("all-orders")]
        public async Task<IActionResult> ViewAllOrders()
        {
            try
            {
                var orders = await _orderRepository.GetAllOrdersAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(id);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("top-10-shops")]
        public async Task<IActionResult> GetTop10Shops()
        {
            try
            {
                var topShops = await _orderRepository.GetTop10ShopsAsync();
                return Ok(topShops);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("order-statistics")]
        public async Task<IActionResult> GetOrderStatistics()
        {
            try
            {
                var stats = await _orderRepository.GetOrderStatisticsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("owner-statistics/{ownerId}")]
        public async Task<IActionResult> GetOrderOwnerStatistics(int ownerId)
        {
            try
            {
                var ownerStats = await _orderRepository.GetOwnerStatisticsAsync(ownerId);
                return Ok(ownerStats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        private string GenerateCodeOrder()
        {
            var random = new Random();
            return new string(Enumerable.Repeat("0123456789", 10).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private void RemoveCartItemFromSession(int userId, string productSizeId)
        {
            var cart = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<CartOwner>>($"Cart_{userId}") ?? new List<CartOwner>();

            foreach (var ownerCart in cart)
            {
                var existingItem = ownerCart.CartItems.FirstOrDefault(i => i.ProductSizeId == productSizeId);
                if (existingItem != null)
                {
                    ownerCart.CartItems.Remove(existingItem);
                    if (ownerCart.CartItems.Count == 0)
                    {
                        cart.Remove(ownerCart);
                    }
                    break;
                }
            }

            _httpContextAccessor.HttpContext.Session.SetObjectAsJson($"Cart_{userId}", cart);
        }
    }
}
