using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Service;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "User")]
    public class CartController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductSizeRepository _productSizeRepository;
        private readonly ICartService _cartService;

        public CartController(IProductRepository productRepository, IProductSizeRepository productSizeRepository, ICartService cartService)
        {
            _productRepository = productRepository;
            _productSizeRepository = productSizeRepository;
            _cartService = cartService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            try
            {
                var cart = await _cartService.GetCartFromCookie(userId) ?? new List<CartOwner>();

                // Calculate the total count of items in the cart
                int totalCount = cart.Sum(owner => owner.CartItems.Sum(item => item.Quantity));

                var cartDto = new CartDTO
                {
                    CartOwners = cart,
                    TotalCount = totalCount
                };
                return Ok(cartDto);
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
                    return StatusCode(404,  new { message = "The product has not been found.." });
                }

                var cart = await _cartService.GetCartFromCookie(userId) ?? new List<CartOwner>();

                var ownerCart = cart.FirstOrDefault(o => o.OwnerId == ownerId);
                if (ownerCart == null)
                {
                    ownerCart = new CartOwner {
                        OwnerId = ownerId,
                        OwnerName = productsize.Product.Owner.Fullname, 
                    };
                    
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
                        TotalPrice = quantity * productsize.Product.Price,
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

                await _cartService.SaveCartToCookie(userId, cart);

                return Ok(new { message = "Product is add to cart successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateCart(int userId, string productsizeId, int quantity, int ownerId)
        {
            try
            {
                var cart = await _cartService.GetCartFromCookie(userId) ?? new List<CartOwner>();

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

                await _cartService.SaveCartToCookie(userId, cart);
                return Ok(new { message = "You have just updated product form cart successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpDelete("{userId}/{productsizeId}")]
        public async Task<IActionResult> DeleteFromCart(int userId, string productsizeId)
        {
            try
            {
                var cart = await _cartService.GetCartFromCookie(userId);
                cart = await _cartService.RemoveCartItemFromCookie(cart, productsizeId);
                await _cartService.SaveCartToCookie(userId,cart);
                return Ok(new { message = "You have just deleted product form cart successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}
