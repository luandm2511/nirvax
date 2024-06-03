using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Helpers;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public CartController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            try
            { 
                var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>($"Cart_{userId}") ?? new List<CartItem>();
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> AddToCart(int userId, string productsizeId, int quantity)
        {
            try
            {
                var productsize = await _productRepository.GetByIdAsync(productsizeId);
                if (productsize == null || productsize.Isdelete == true)
                {
                    return NotFound(new { message = "Product is not found." });
                }

                var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>($"Cart_{userId}") ?? new List<CartItem>();

                var cartItem = cart.FirstOrDefault(c => c.ProductSizeId == productsizeId);
                if (cartItem == null)
                {
                    cart.Add(new CartItem
                    {
                        ProductSizeId = productsize.ProductSizeId,
                        ProductId = productsize.ProductId,
                        ProductName = productsize.Product.Name,
                        SizeName = productsize.Size.Name,
                        Price = productsize.Product.Price,
                        Quantity = quantity,
                        ImageUrl = productsize.Product.Images.FirstOrDefault()?.LinkImage
                    });
                }
                else
                {
                    cartItem.Quantity += quantity;
                }

                HttpContext.Session.SetObjectAsJson($"Cart_{userId}", cart);

                return Ok(new { message = "Product is created successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPut("{userId}")]
        public IActionResult UpdateCart(int userId, string productsizeId, int quantity)
        {
            try
            { 
                var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>($"Cart_{userId}") ?? new List<CartItem>();
                var cartItem = cart.FirstOrDefault(c => c.ProductSizeId == productsizeId);

                if (cartItem == null)
                {
                    return NotFound($"Don't have any product by {productsizeId} in cart.");
                }

                if (quantity <= 0)
                {
                    cart.Remove(cartItem);
                }
                else
                {
                    cartItem.Quantity = quantity;
                }

                HttpContext.Session.SetObjectAsJson($"Cart_{userId}", cart);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpDelete("{userId}/{productsizeId}")]
        public IActionResult DeleteFromCart(int userId, string productsizeId)
        {
            try
            {
                var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>($"Cart_{userId}") ?? new List<CartItem>();
                var cartItem = cart.FirstOrDefault(c => c.ProductSizeId == productsizeId);

                if (cartItem == null)
                {
                    return NotFound("Don't have any product in cart.");
                }

                cart.Remove(cartItem);

                HttpContext.Session.SetObjectAsJson($"Cart_{userId}", cart);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }
    }
}
