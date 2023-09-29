using Restaurant.Service.ShoppingCartAPI.Models.Dto;

namespace Restaurant.Services.ShoppingCartAPI.Repository.IRepository
{
    public interface ICouponRepository
    {
        Task<CouponDto> GetCoupon(string couponName);
    }
}
