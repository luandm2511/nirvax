using BusinessObject.DTOs;
using Newtonsoft.Json;

namespace WebAPI.Service
{
    public class CartService : ICartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public List<CartOwner> GetCartFromCookie(int userId)
        {
            var cookie = _httpContextAccessor.HttpContext.Request.Cookies[$"Cart_{userId}"];
            if (cookie == null)
            {
                return new List<CartOwner>();
            }
            return JsonConvert.DeserializeObject<List<CartOwner>>(cookie);
        }

        public void SaveCartToCookie(int userId, List<CartOwner> cart)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(30),
                HttpOnly = true,
                IsEssential = true
            };
            var cartJson = JsonConvert.SerializeObject(cart);
            _httpContextAccessor.HttpContext.Response.Cookies.Append($"Cart_{userId}", cartJson, cookieOptions);
        }

        public void RemoveCartItemFromCookie(int userId, string productSizeId)
        {
            var cart = GetCartFromCookie(userId) ?? new List<CartOwner>();

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

            SaveCartToCookie(userId, cart);
        }
    }
}
