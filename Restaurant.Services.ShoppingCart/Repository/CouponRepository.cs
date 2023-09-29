using Newtonsoft.Json;
using Restaurant.Service.ShoppingCartAPI.Models.Dto;
using Restaurant.Services.ShoppingCartAPI.Models.Dto;
using Restaurant.Services.ShoppingCartAPI.Repository.IRepository;

namespace Restaurant.Service.ShoppingCartAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly HttpClient _httpClient;

        public CouponRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CouponDto> GetCoupon(string couponName)
        {
            var response = await _httpClient.GetAsync($"/api/CouponAPI/{couponName}");
            var apiContent = await response.Content.ReadAsStringAsync();
            var responseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
        
            if (responseDto.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(responseDto.Result));
            }

            return new CouponDto();
        }
    }
}
