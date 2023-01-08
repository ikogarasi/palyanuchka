using Microsoft.EntityFrameworkCore;
using Restaurant.Service.CouponAPI.Models;

namespace Restaurant.Service.CouponAPI.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Coupon> Coupons { get; set; }
    }
}
