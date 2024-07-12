using BusinessObject.DTOs;

namespace WebAPI.Service
{
    public interface ICartService
    {
        List<CartOwner> GetCartFromCookie(int userId);
        void SaveCartToCookie(int userId, List<CartOwner> cart);
        void RemoveCartItemFromCookie(int userId, string productSizeId);
    }
}
