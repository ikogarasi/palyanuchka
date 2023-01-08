using RestaurantWeb.Models;

namespace RestaurantWeb.Services.IServices
{
    public interface IShoppingCartService
    {
        Task<T> GetCartByUserIdAsync<T>(string userId, string accessToken = null);
        Task<T> AddToCartAsync<T>(CartDto cartDto, string accessToken = null);
        Task<T> UpdateCartAsync<T>(CartDto cartDto, string token = null);
        Task<T> RemoveFromCartAsync<T>(int cartId, string token = null);
        Task<T> ApplyCoupon<T>(CartDto cartDto, string token = null);
        Task<T> RemoveCoupon<T>(string userId, string token = null);
        Task<T> Checkout<T>(CartHeaderDto cartHeader, string token = null);
    }
}
