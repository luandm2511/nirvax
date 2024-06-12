using System.Linq;
using AutoMapper;
using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDTO createOrderDTO)
        {
            try
            {
                DateTime orderDate = DateTime.UtcNow;

                var userId = createOrderDTO.AccountId;
                var cart = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<CartOwner>>($"Cart_{userId}") ?? new List<CartOwner>();

                // Lấy danh sách sản phẩm được chọn từ giỏ hàng
                var selectedItems = cart.SelectMany(owner => owner.CartItems)
                                        .Where(item => createOrderDTO.CartItemIds.Contains(item.ProductSizeId))
                                        .ToList();

                // Group các sản phẩm theo OwnerId
                var groupedItems = selectedItems.GroupBy(item => item.OwnerId);

                var orders = new List<Order>();

                foreach (var group in groupedItems)
                {
                    int ownerId = group.Key;
                    double ownerTotalAmount = 0;

                    string codeOrder;
                    do
                    {
                        codeOrder = GenerateCodeOrder();
                    } while (await _orderRepository.CodeOrderExists(codeOrder));

                    var order = new Order
                    {
                        CodeOrder = codeOrder,
                        OrderDate = orderDate,
                        RequiredDate = createOrderDTO.RequiredDate,
                        AccountId = createOrderDTO.AccountId,
                        OwnerId = ownerId,
                        StatusId = 1, // Assuming 1 is for pending status
                        VoucherId = createOrderDTO.VoucherId
                    };

                    await _orderRepository.AddOrder(order);

                    foreach (var cartItem in group)
                    {
                        var productSize = await _productSizeRepository.GetByIdAsync(cartItem.ProductSizeId);
                        if (productSize != null && productSize.Quantity >= cartItem.Quantity)
                        {
                            productSize.Quantity -= cartItem.Quantity;
                            await _productSizeRepository.UpdateAsync(productSize);

                            ownerTotalAmount += cartItem.Price * cartItem.Quantity;

                            // Add order details
                            var orderDetail = new OrderDetail
                            {
                                ProductSizeId = cartItem.ProductSizeId,
                                Quantity = cartItem.Quantity,
                                UnitPrice = cartItem.Price,
                                OrderId = order.OrderId
                            };
                            await _orderDetailRepository.AddOrderDetailAsync(orderDetail);
                        }
                        else
                        {
                            return BadRequest($"Insufficient quantity for ProductSizeId {cartItem.ProductSizeId}");
                        }
                    }
                    var notification = new Notification
                    {
                        AccountId = null,
                        OwnerId = ownerId, // Assuming Product model has OwnerId field
                        Content = $"You has just had a order with code order {codeOrder}.",
                        IsRead = false,
                        Url = null,
                        CreateDate = DateTime.UtcNow
                    };

                    var notificationResult = await _notificationRepository.AddNotificationAsync(notification);
                    if (!notificationResult)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to create the notification." });
                    }

                    // Trừ tiền khi có voucher
                    if (!string.IsNullOrEmpty(createOrderDTO.VoucherId))
                    {
                        var voucher = await _voucherRepository.GetVoucherById(createOrderDTO.VoucherId);
                        if (voucher == null || voucher.Isdelete == true || voucher.Quantity <= 0 || voucher.EndDate > DateTime.Now)
                        {
                            return BadRequest($"Voucher with {voucher.VoucherId} is not found");
                        }
                        ownerTotalAmount -= voucher.Price;
                    }

                    order.TotalAmount = ownerTotalAmount;
                    orders.Add(order);
                }

                // Xóa các sản phẩm đã được đặt hàng ra khỏi giỏ hàng
                foreach (var owner in cart)
                {
                    owner.CartItems.RemoveAll(item => selectedItems.Any(si => si.ProductSizeId == item.ProductSizeId));
                }
                cart.RemoveAll(owner => owner.CartItems.Count == 0);
                _httpContextAccessor.HttpContext.Session.SetObjectAsJson($"Cart_{userId}", cart);

                return Ok(new { Orders = _mapper.Map<IEnumerable<OrderDTO>>(orders) });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("order-now")]
        public async Task<IActionResult> OrderNow([FromBody] OrderNowDTO orderNowDTO)
        {
            try
            {
                DateTime orderDate = DateTime.UtcNow;

                var productSize = await _productSizeRepository.GetByIdAsync(orderNowDTO.ProductSizeId);
                if (productSize == null || productSize.Quantity < orderNowDTO.Quantity)
                {
                    return BadRequest("Insufficient quantity.");
                }

                productSize.Quantity -= orderNowDTO.Quantity;
                await _productSizeRepository.UpdateAsync(productSize);

                double totalAmount = productSize.Product.Price * orderNowDTO.Quantity;

                // Trừ tiền khi có voucher
                if (!string.IsNullOrEmpty(orderNowDTO.VoucherId))
                {
                    var voucher = await _voucherRepository.GetVoucherById(orderNowDTO.VoucherId);
                    if (voucher == null || voucher.Isdelete == true || voucher.Quantity <= 0 || voucher.EndDate > DateTime.Now)
                    {
                        return BadRequest($"Voucher with {voucher.VoucherId} is not found");
                    }
                    totalAmount -= voucher.Price;
                }

                string codeOrder;
                do
                {
                    codeOrder = GenerateCodeOrder();
                } while (await _orderRepository.CodeOrderExists(codeOrder));

                var order = new Order
                {
                    CodeOrder = codeOrder,
                    OrderDate = orderDate,
                    RequiredDate = orderNowDTO.RequiredDate,
                    AccountId = orderNowDTO.AccountId,
                    OwnerId = productSize.Product.OwnerId,
                    TotalAmount = totalAmount,
                    StatusId = 1, // Assuming 1 is for pending status
                    VoucherId = orderNowDTO.VoucherId
                };

                await _orderRepository.AddOrder(order);

                // Add order detail
                var orderDetail = new OrderDetail
                {
                    ProductSizeId = orderNowDTO.ProductSizeId,
                    Quantity = orderNowDTO.Quantity,
                    UnitPrice = productSize.Product.Price,
                    OrderId = order.OrderId
                };
                await _orderDetailRepository.AddOrderDetailAsync(orderDetail);

                var notification = new Notification
                {
                    AccountId = null,
                    OwnerId = productSize.Product.OwnerId, // Assuming Product model has OwnerId field
                    Content = $"You has just had a order with code order {codeOrder}.",
                    IsRead = false,
                    Url = null,
                    CreateDate = DateTime.UtcNow
                };

                var notificationResult = await _notificationRepository.AddNotificationAsync(notification);
                if (!notificationResult)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to create the notification." });
                }

                return Ok("Order successfull");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("history/{accountId}")]
        public async Task<IActionResult> ViewOrderHistory(int accountId)
        {
            try
            {
                var orders = await _orderRepository.GetOrdersByAccountId(accountId);
                var orderDTOs = _mapper.Map<IEnumerable<OrderDTO>>(orders);
                return Ok(orderDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("owner/{ownerId}")]
        public async Task<IActionResult> ViewOrdersByOwner(int ownerId)
        {
            try
            {
                var orders = await _orderRepository.GetOrdersByOwnerId(ownerId);
                var orderDTOs = _mapper.Map<IEnumerable<OrderDTO>>(orders);
                return Ok(orderDTOs);
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
                var order = await _orderRepository.GetOrderById(orderId);
                if (order == null)
                {
                    return NotFound();
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
                }

                order.StatusId = statusId; 
                await _orderRepository.UpdateOrder(order);

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

                var notificationResult = await _notificationRepository.AddNotificationAsync(notification);
                if (!notificationResult)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to create the notification." });
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchOrders([FromQuery] string codeOrder)
        {
            try
            {
                var orders = await _orderRepository.SearchOrders(codeOrder);
                var orderDTOs = _mapper.Map<IEnumerable<OrderDTO>>(orders);
                return Ok(orderDTOs);
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
