using BusinessObject.DTOs;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailRepository _orderDetailRepository;
        public OrderDetailController(IOrderDetailRepository orderDetailRepository)
        {
            _orderDetailRepository = orderDetailRepository;
        }

        [HttpGet("{orderId}")]
        //[Authorize(Roles = "User,Owner,Staff")]
        public async Task<IActionResult> GetOrderDetail(int orderId)
        {
            try
            {
               var orderDetail = await _orderDetailRepository.GetHistoryOrderDetailsByOrderIdAsync(orderId);
               return Ok(orderDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}

