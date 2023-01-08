using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Restaurant.Service.CouponAPI.DbContexts;
using Restaurant.Service.CouponAPI.Models.Dto;
using Restaurant.Service.CouponAPI.Repository.IRepository;

namespace Restaurant.Service.CouponAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContext _db;
        private IMapper _mapper;

        public CouponRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<CouponDto> GetCouponByCode(string code)
        {
            var dto = await _db.Coupons.FirstOrDefaultAsync(i => i.CouponCode == code);
            return _mapper.Map<CouponDto>(dto);
        }
    }
}
