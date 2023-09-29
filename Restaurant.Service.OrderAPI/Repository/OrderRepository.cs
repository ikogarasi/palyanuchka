using Microsoft.EntityFrameworkCore;
using Restaurant.Service.OrderAPI.DbContexts;
using Restaurant.Service.OrderAPI.Models;
using Restaurant.Service.OrderAPI.Repository.IRepository;

namespace Restaurant.Service.OrderAPI.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContext;

        public OrderRepository(DbContextOptions<ApplicationDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddOrder(OrderHeader order)
        {
            await using var _db = new ApplicationDbContext(_dbContext);
            _db.OrderHeaders.Add(order);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task UpdateOrderPaymentStatus(int orderHeaderId, bool paid)
        {
            await using var _db = new ApplicationDbContext(_dbContext);
            var orderHeaderFromDb = await _db.OrderHeaders.FirstOrDefaultAsync(i => i.OrderHeaderId == orderHeaderId);
            if (orderHeaderFromDb != null)
            {
                orderHeaderFromDb.PaymentStatus = paid;
                await _db.SaveChangesAsync();
            }

        }
    }
}
