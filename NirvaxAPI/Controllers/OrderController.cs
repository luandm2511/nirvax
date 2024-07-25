using System.Linq;
using AutoMapper;
using Azure.Core;
using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.DAOs;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebAPI.Helpers;
using WebAPI.Service;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IProductSizeRepository _productSizeRepository;
        private readonly IProductRepository _productRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IVoucherRepository _voucherRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        ICartService _cartService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderController(
            IHubContext<NotificationHub> hubContext,
            IOrderRepository orderRepository,
            IOrderDetailRepository orderDetailRepository,
            IProductSizeRepository productSizeRepository,
            IProductRepository productRepository,
            INotificationRepository notificationRepository,
            IVoucherRepository voucherRepository,
            ITransactionRepository transactionRepository,
            ICartService cartService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _hubContext = hubContext;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _productSizeRepository = productSizeRepository;
            _productRepository = productRepository;
            _notificationRepository = notificationRepository;
            _voucherRepository = voucherRepository;
            _transactionRepository = transactionRepository;
            _cartService = cartService;
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

            if (voucher != null&& (voucher.OwnerId != request.OwnerId || voucher.EndDate < DateTime.Now 
                || DateTime.Now < voucher.StartDate || voucher.Quantity <= voucher.QuantityUsed || voucher.VoucherId != request.VoucherId))
            {
                return BadRequest($"{request.VoucherId} is invalid.");
            }

            return Ok(voucher);
        }

        [HttpPost]
        //[Authorize(Roles = "User")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDTO createOrderDTO)
        {
            using var transaction = await _transactionRepository.BeginTransactionAsync();
            try
            {
                var cart = await _cartService.GetCartFromCookie(createOrderDTO.AccountId);

                // Group các sản phẩm theo OwnerId
                var groupedItems = createOrderDTO.Items.GroupBy(item => item.OwnerId);

                // Validate vouchers nếu vouchers không null hoặc rỗng
                if (createOrderDTO.Vouchers != null && createOrderDTO.Vouchers.Any())
                {
                    foreach (var voucherDto in createOrderDTO.Vouchers)
                    {
                        if (!string.IsNullOrEmpty(voucherDto.VoucherId))
                        {
                            var voucher = await _voucherRepository.GetVoucherById(voucherDto.VoucherId);
                            if (voucher == null || voucher.OwnerId != voucherDto.OwnerId || voucher.EndDate < DateTime.Now)
                            {
                                return BadRequest($"Voucher [{voucherDto.VoucherId}] is invalid.");
                            }
                        }
                    }
                }

                var orders = new List<Order>();

                foreach (var group in groupedItems)
                {
                    var ownerVoucherDto = createOrderDTO.Vouchers?.FirstOrDefault(v => v.OwnerId == group.Key);
                    double voucherPrice = 0;
                    string ownerVoucherId = null;
                    string ownerVoucherNote = null;

                    if (ownerVoucherDto != null)
                    {
                        if (!string.IsNullOrEmpty(ownerVoucherDto.VoucherId))
                        {
                            var voucher = await _voucherRepository.PriceAndQuantityByOrderAsync(ownerVoucherDto.VoucherId);
                            voucherPrice = voucher?.Price ?? 0;
                            ownerVoucherId = ownerVoucherDto.VoucherId;
                        }
                        ownerVoucherNote = ownerVoucherDto.Note;
                    }

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
                        OrderDate = DateTime.Now,
                        Address = createOrderDTO.Address,
                        Note = ownerVoucherNote,
                        TotalAmount = group.Sum(item => item.Quantity * item.UnitPrice) - voucherPrice,
                        AccountId = createOrderDTO.AccountId,
                        OwnerId = group.Key,
                        StatusId = 1,
                        VoucherId = ownerVoucherId
                    };

                    await _orderRepository.AddOrderAsync(order);

                    foreach (var cartItem in group)
                    {
                        var productSize = await _productSizeRepository.GetByIdAsync(cartItem.ProductSizeId);
                        if (productSize != null && productSize.Quantity >= cartItem.Quantity)
                        {
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
                                cart = await _cartService.RemoveCartItemFromCookie(cart, cartItem.ProductSizeId);
                            }
                        }
                        else
                        {
                            await _transactionRepository.RollbackTransactionAsync();
                            return BadRequest($"Insufficient quantity for ProductSizeId {cartItem.ProductSizeId}");
                        }
                    }

                    var notification = new Notification
                    {
                        AccountId = null,
                        OwnerId = group.Key, // Assuming Product model has OwnerId field
                        Content = $"You have just had an order with code order {codeOrder}.",
                        IsRead = false,
                        Url = null,
                        CreateDate = DateTime.Now
                    };

                    await _notificationRepository.AddNotificationAsync(notification);
                    await _cartService.SaveCartToCookie(order.AccountId, cart);
                    // Gửi thông báo cho chủ sở hữu sản phẩm
                    await _hubContext.Clients.Group($"Owner-{group.Key}").SendAsync("ReceiveNotification", notification.Content);
                }

                await _transactionRepository.CommitTransactionAsync();

                return Ok(new { message = "Order successful!" });
            }
            catch (Exception ex)
            {
                await _transactionRepository.RollbackTransactionAsync();
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
            using var transaction = await _transactionRepository.BeginTransactionAsync();
            try
            {
                var content = "";
                var order = await _orderRepository.GetOrderByIdAsync(orderId);
                if (order == null)
                {
                    return NotFound();
                }

                if(statusId == 2)
                {
                    order.RequiredDate = DateTime.Now;
                    content = $"You have an order with code {order.CodeOrder} that has been delivered to the shipping company by the seller.";
                }

                if (statusId == 3)
                {
                    var orderDetail = await _orderDetailRepository.GetOrderDetailsByOrderIdAsync(orderId);
                    foreach (var detail in orderDetail)
                    {
                        var product = await _productRepository.GetByIdAsync(detail.ProductSize.ProductId);
                        if (product != null)
                        {
                            product.QuantitySold += detail.Quantity;
                            await _productRepository.UpdateAsync(product);
                        }
                    }
                    order.ShippedDate = DateTime.Now;
                    content = $"You have an order with code {order.CodeOrder} that has been successfully delivered.";
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
                    order.ShippedDate = DateTime.Now;
                    content = $"You have an order with code {order.CodeOrder} that has failed to be delivered.";
                }

                order.StatusId = statusId; 
                await _orderRepository.UpdateOrderAsync(order);

                // Notify user about order confirmation (implementation skipped)
                var notification = new Notification
                {
                    AccountId = order.AccountId,
                    OwnerId = null, // Assuming Product model has OwnerId field
                    Content = content ,
                    IsRead = false,
                    Url = "abcd",
                    CreateDate = DateTime.Now
                };

                await _notificationRepository.AddNotificationAsync(notification);
                await _transactionRepository.CommitTransactionAsync();
                // Gửi thông báo cho người dùng và chủ sở hữu sản phẩm
                await _hubContext.Clients.Group($"User-{order.AccountId}").SendAsync("ReceiveNotification", notification.Content);
                return Ok();
            }
            catch (Exception ex)
            {
                await _transactionRepository.RollbackTransactionAsync();
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("cancel/{orderId}")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            using var transaction = await _transactionRepository.BeginTransactionAsync();
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
                    Url = "abcd",
                    CreateDate = DateTime.Now
                };

                await _notificationRepository.AddNotificationAsync(notification);
                await _transactionRepository.CommitTransactionAsync();
                // Gửi thông báo cho chủ sở hữu sản phẩm
                await _hubContext.Clients.Group($"Owner-{order.OwnerId}").SendAsync("ReceiveNotification", notification.Content);
                return Ok();
            }
            catch (Exception ex)
            {
                await _transactionRepository.RollbackTransactionAsync();
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("confirm-success/{orderId}")]
        public async Task<IActionResult> SuccessOrder(int orderId)
        {
            using var transaction = await _transactionRepository.BeginTransactionAsync();
            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(orderId);
                if (order == null)
                {
                    return NotFound();
                }
                var orderDetail = await _orderDetailRepository.GetOrderDetailsByOrderIdAsync(orderId);
                foreach (var detail in orderDetail)
                {
                    var product = await _productRepository.GetByIdAsync(detail.ProductSize.ProductId);
                    if (product != null)
                    {
                        product.QuantitySold += detail.Quantity;
                        await _productRepository.UpdateAsync(product);
                    }
                }

                order.ShippedDate = DateTime.Now;
                order.StatusId = 3;
                await _orderRepository.UpdateOrderAsync(order);

                var notification = new Notification
                {
                    AccountId = null,
                    OwnerId = order.OwnerId,
                    Content = $"The order with order code {order.CodeOrder} has been successfully confirmed by user.",
                    IsRead = false,
                    Url = "abcd",
                    CreateDate = DateTime.Now
                };

                await _notificationRepository.AddNotificationAsync(notification);
                await _transactionRepository.CommitTransactionAsync();
                // Gửi thông báo cho chủ sở hữu sản phẩm
                await _hubContext.Clients.Group($"Owner-{order.OwnerId}").SendAsync("ReceiveNotification", notification.Content);
                return Ok();
            }
            catch (Exception ex)
            {
                await _transactionRepository.RollbackTransactionAsync();
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
    }
}
