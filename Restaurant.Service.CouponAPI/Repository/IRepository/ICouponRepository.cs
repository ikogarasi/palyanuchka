using Restaurant.Service.CouponAPI.Models.Dto;

namespace Restaurant.Service.CouponAPI.Repository.IRepository
{
    public interface ICouponRepository
    {
        Task<CouponDto> GetCouponByCode(string code);
    }
}
