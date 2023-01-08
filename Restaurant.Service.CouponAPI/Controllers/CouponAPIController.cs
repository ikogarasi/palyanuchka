using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Service.CouponAPI.Repository.IRepository;
using Restaurant.Services.CouponAPI.Models.Dto;

namespace Restaurant.Service.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly ICouponRepository _couponRepository;
        private ResponseDto _response;

        public CouponAPIController(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
            _response = new();
        }

        [HttpGet("{couponCode}")]
        public async Task<ResponseDto> GetCoupon(string couponCode)
        {
            try
            {
                var coupon = await _couponRepository.GetCouponByCode(couponCode);
                _response.Result = coupon;
            }
            catch(Exception ex)
            {
                _response.ErrorMessages.Add(ex.Message);
                _response.IsSuccess = false;
            }

            return _response;
        }
    }
}
