using BusinessObject.DTOs;

namespace WebAPI.Service
{
    public interface ICartService
    {
        Task<List<CartOwner>> GetCartFromCookie(int userId);
        Task SaveCartToCookie(int userId, List<CartOwner> cart);
        Task<List<CartOwner>> RemoveCartItemFromCookie(List<CartOwner> cart, string productSizeId);
    }
}
