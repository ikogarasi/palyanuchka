using RestaurantWeb.Models;
using RestaurantWeb.Services.IServices;

namespace RestaurantWeb.Services
{
    public class CouponService : BaseService, ICouponService
    {
        private readonly IHttpClientFactory _httpClient;

        public CouponService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClient = httpClientFactory;
        }

        public async Task<T> GetCoupon<T>(string couponCode, string token = null)
        {
            return await this.SendAsync<T>(
                new ApiRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = SD.CouponAPIBase + $"/api/CouponAPI/{couponCode}",
                    AccessToken = token
                }
            );
        }
    }
}
