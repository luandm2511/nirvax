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
        private readonly IProductSizeRepository _productSizeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartController(IProductRepository productRepository,IProductSizeRepository productSizeRepository, IHttpContextAccessor httpContextAccessor)
        {
            _productRepository = productRepository;
            _productSizeRepository = productSizeRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            try
            { 
                var cart = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<CartOwner>>($"Cart_{userId}") ?? new List<CartOwner>();
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> AddToCart(int userId, string productsizeId, int quantity, int ownerId)
        {
            try
            {
                var productsize = await _productSizeRepository.GetByIdAsync(productsizeId);
                if (productsize == null || productsize.Isdelete == true)
                {
                    return NotFound(new { message = "Product is not found." });
                }

                var cart = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<CartOwner>>($"Cart_{userId}") ?? new List<CartOwner>();

                var ownerCart = cart.FirstOrDefault(o => o.OwnerId == ownerId);
                if (ownerCart == null)
                {
                    ownerCart = new CartOwner { OwnerId = ownerId };
                    cart.Add(ownerCart);
                }

                var existingItem = ownerCart.CartItems.FirstOrDefault(c => c.ProductSizeId == productsizeId);
                if (existingItem == null)
                {
                    ownerCart.CartItems.Add(new CartItem
                    {
                        ProductSizeId = productsize.ProductSizeId,
                        ProductName = productsize.Product.Name,
                        Size = productsize.Size.Name,
                        Price = productsize.Product.Price,
                        Quantity = quantity,
                        Image = productsize.Product.Images.FirstOrDefault()?.LinkImage,
                        OwnerId = ownerId
                    });
                }
                else
                {
                    existingItem.Quantity += quantity;
                }
                // Move the updated owner to the top of the cart list
                cart.Remove(ownerCart);
                cart.Insert(0, ownerCart);

                _httpContextAccessor.HttpContext.Session.SetObjectAsJson($"Cart_{userId}", cart);

                return Ok(new { message = "Product is add to cart successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPut("{userId}")]
        public IActionResult UpdateCart(int userId, string productsizeId, int quantity, int ownerId)
        {
            try
            {
                var cart = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<CartOwner>>($"Cart_{userId}") ?? new List<CartOwner>();
                
                var ownerCart = cart.FirstOrDefault(o => o.OwnerId == ownerId);
                if (ownerCart != null)
                {
                    var existingItem = ownerCart.CartItems.FirstOrDefault(i => i.ProductSizeId == productsizeId);
                    if (existingItem != null)
                    {
                        existingItem.Quantity = quantity;
                        if (existingItem.Quantity <= 0)
                        {
                            ownerCart.CartItems.Remove(existingItem);
                        }
                    }
                    if (ownerCart.CartItems.Count == 0)
                    {
                        cart.Remove(ownerCart);
                    }
                }


                _httpContextAccessor.HttpContext.Session.SetObjectAsJson($"Cart_{userId}", cart);
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
                var cart = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<CartOwner>>($"Cart_{userId}") ?? new List<CartOwner>();

                foreach (var ownerCart in cart)
                {
                    var existingItem = ownerCart.CartItems.FirstOrDefault(i => i.ProductSizeId == productsizeId);
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
