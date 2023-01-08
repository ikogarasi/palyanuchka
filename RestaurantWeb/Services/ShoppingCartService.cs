using RestaurantWeb.Models;
using RestaurantWeb.Services.IServices;

namespace RestaurantWeb.Services
{
    public class ShoppingCartService : BaseService, IShoppingCartService
    {
        private readonly IHttpClientFactory _clientFactory;

        public ShoppingCartService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _clientFactory = httpClientFactory;
        }

        public async Task<T> AddToCartAsync<T>(CartDto cartDto, string accessToken = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.ShoppingCartAPIBase + "/api/carts/AddCart",
                AccessToken = accessToken
            });
        }

        public async Task<T> GetCartByUserIdAsync<T>(string userId, string accessToken = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ShoppingCartAPIBase + "/api/carts/GetCart/" + userId,
                AccessToken = accessToken
            });
        }

        public async Task<T> RemoveFromCartAsync<T>(int cartId, string accessToken = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.ShoppingCartAPIBase + "/api/carts/RemoveCart/?cartId=" + cartId,
                AccessToken = accessToken
            });
        }

        public async Task<T> UpdateCartAsync<T>(CartDto cartDto, string accessToken = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = cartDto,
                Url = SD.ShoppingCartAPIBase + "/api/carts/UpdateCart",
                AccessToken = accessToken
            });
        }

        public async Task<T> ApplyCoupon<T>(CartDto cartDto, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.ShoppingCartAPIBase + "/api/carts/ApplyCoupon",
                AccessToken = token
            });
        }

        public async Task<T> RemoveCoupon<T>(string userId, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = userId,
                Url = SD.ShoppingCartAPIBase + "/api/carts/RemoveCoupon",
                AccessToken = token
            });
        }

        public async Task<T> Checkout<T>(CartHeaderDto cartHeader, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = cartHeader,
                Url = SD.ShoppingCartAPIBase + "/api/carts/Checkout",
                AccessToken = token
            });

        }
    }
}
