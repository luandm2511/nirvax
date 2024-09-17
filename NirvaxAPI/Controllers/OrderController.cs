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
        private readonly ICartService _cartService;
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
            try
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
                            OwnerId = productSize.Product.OwnerId,
                            OwnerName = productSize.Product.Owner.Fullname,
                            Image = productSize.Product.Images.Where(i=>i.ProductId == productSize.ProductId).FirstOrDefault()?.LinkImage,
                        });
                    }
                    else
                    {
                        return StatusCode(404, $"The {productSize.Product.Name} you purchased is not found or the quantity in stock is not enough for the product you want to order.");
                    }
                }

                return Ok(orderItemDetails);
            }
            catch (Exception )
            {
                return StatusCode(500, $"Internal server error: {"Something went wrong, please try again."}");
            }
        }

        [HttpPost("check-voucher")]
        public async Task<IActionResult> CheckVoucher([FromBody] VoucherOrderDTO request)
        {
            try
            {
                var voucher = await _voucherRepository.GetVoucherById(request.VoucherId);

                if (voucher == null || (voucher.OwnerId != request.OwnerId || voucher.EndDate < DateTime.Now
                    || DateTime.Now < voucher.StartDate || voucher.Quantity <= voucher.QuantityUsed || voucher.VoucherId != request.VoucherId))
                {
                    return StatusCode(400, new { message = $"The voucher code is invalid." });
                }

                return Ok(voucher);
            }
            catch (Exception )
            {
                return StatusCode(500, $"Internal server error: {"Something went wrong, please try again."}");
            } 
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

                var orders = new List<Order>();

                foreach (var group in groupedItems)
                {
                    var itemGroup = new ItemGroup{
                        OwnerId = group.Key,
                        Items = group
                    };
                    var order = await _orderRepository.AddOrderAsync(createOrderDTO, itemGroup);

                    foreach (var cartItem in group)
                    {
                            // Add order details
                            var orderDetail = new OrderDetail
                            {
                                ProductSizeId = cartItem.ProductSizeId,
                                Quantity = cartItem.Quantity,
                                UnitPrice = cartItem.UnitPrice,
                                OrderId = order.OrderId,
                                UserRate = 0                            
                            };
                            await _orderDetailRepository.AddOrderDetailAsync(orderDetail);
                            // Remove item from session cart
                            if (!createOrderDTO.IsOrderNow)
                            {
                                cart = await _cartService.RemoveCartItemFromCookie(cart, cartItem.ProductSizeId);
                            }
                    }

                    var notification = new Notification
                    {
                        AccountId = null,
                        OwnerId = group.Key, // Assuming Product model has OwnerId field
                        Content = $"You have just had an order with code order {order.CodeOrder}.",
                        IsRead = false,
                        Url = $"{order.OrderId}",
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
            catch (Exception e)
            {
                await _transactionRepository.RollbackTransactionAsync();
                return StatusCode(500, $"Internal server error: {e.Message}");
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
            catch (Exception )
            {
                return StatusCode(500, $"Internal server error: {"Something went wrong, please try again."}");
            }
        }

        [HttpGet("owner/{ownerId}")]
        //[Authorize(Roles = "Owner,Staff")]
        public async Task<IActionResult> ViewOrdersByOwner(int ownerId) 
        {
            try
            {
                var orders = await _orderRepository.GetOrdersByOwnerIdAsync(ownerId);
                return Ok(orders);
            }
            catch (Exception )
            {
                return StatusCode(500, $"Internal server error: {"Something went wrong, please try again."}");
            }
        }

        [HttpPut("confirm/{orderId}/{statusId}")]

        public async Task<IActionResult> ConfirmOrder(int orderId, int statusId)
        {
            using var transaction = await _transactionRepository.BeginTransactionAsync();
            try
            {
                var content = "";
                var order = new Order();

                if(statusId == 2)
                {
                    order = await _orderRepository.ConfirmOrder(orderId);
                    content = $"You have an order with code {order.CodeOrder} that has been delivered to the shipping company by the seller.";
                }

                if (statusId == 3)
                {
                    order = await _orderRepository.SucessOrder(orderId);
                    content = $"You have an order with code {order.CodeOrder} that has been successfully delivered.";
                }

                if (statusId == 4)
                {
                    order = await _orderRepository.RejectedOrder(orderId);
                    content = $"You have an order with code {order.CodeOrder} that has failed to be delivered.";
                }
                if (statusId == 6) 
                {
                    order = await _orderRepository.FailedOrder(orderId);
                    content = $"You have an order with code {order.CodeOrder} that has been cancelled.";
                }

                // Notify user about order confirmation (implementation skipped)
                var notification = new Notification
                {
                    AccountId = order.AccountId,
                    OwnerId = null, // Assuming Product model has OwnerId field
                    Content = content ,
                    IsRead = false,
                    Url = $"http://localhost:4200/order-history-details/{orderId}",
                    CreateDate = DateTime.Now
                };

                await _notificationRepository.AddNotificationAsync(notification);
                await _transactionRepository.CommitTransactionAsync();
                // Gửi thông báo cho người dùng và chủ sở hữu sản phẩm
                await _hubContext.Clients.Group($"User-{order.AccountId}").SendAsync("ReceiveNotification", notification.Content);
                return Ok(new { message = "Confirm order successful!" });
            }
            catch (Exception )
            {
                await _transactionRepository.RollbackTransactionAsync();
                return StatusCode(500, $"Internal server error: {"Something went wrong, please try again."}");
            }
        }

        [HttpPut("cancel/{orderId}")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            using var transaction = await _transactionRepository.BeginTransactionAsync();
            try
            {
                var order = await _orderRepository.CancelOrder(orderId);

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
                return Ok(new { message = "You have just canceled successfully order!" });
            }
            catch (Exception )
            {
                await _transactionRepository.RollbackTransactionAsync();
                return StatusCode(500, $"Internal server error: {"Something went wrong, please try again."}");
            }
        }

        [HttpPut("confirm-success/{orderId}")]
        public async Task<IActionResult> SuccessOrder(int orderId)
        {
            using var transaction = await _transactionRepository.BeginTransactionAsync();
            try
            {
                var order = await _orderRepository.SucessOrder(orderId);
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
                return Ok(new { message = "You have just confirmed that you have received your order successfully.!" });
            }
            catch (Exception )
            {
                await _transactionRepository.RollbackTransactionAsync();
                return StatusCode(500, $"Internal server error: {"Something went wrong, please try again."}");
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
            catch (Exception )
            {
                return StatusCode(500, $"Internal server error: {"Something went wrong, please try again."}");
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
            catch (Exception )
            {
                return StatusCode(500, $"Internal server error: {"Something went wrong, please try again."}");
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
            catch (Exception )
            {
                return StatusCode(500, $"Internal server error: {"Something went wrong, please try again."}");
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
            catch (Exception )
            {
                return StatusCode(500, $"Internal server error: {"Something went wrong, please try again."}");
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
            catch (Exception )
            {
                return StatusCode(500, $"Internal server error: {"Something went wrong, please try again."}");
            }
        }
        
    }
}
