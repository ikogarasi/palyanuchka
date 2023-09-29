using Microsoft.EntityFrameworkCore;
using Restaurant.Service.Email.DbContexts;
using Restaurant.Service.Email.Messages;
using Restaurant.Service.Email.Models;
using Restaurant.Service.Email.Repository.IRepository;

namespace Restaurant.Service.Email.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContext;

        public EmailRepository(DbContextOptions<ApplicationDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SendAndLogEmail(UpdatePaymentResultMessage message)
        {
            var emaillLog = new EmailLog()
            {
                Email = message.Email,
                EmailSent = DateTime.Now,
                Log = $"Order - {message.OrderId} has been created successfully."
            };

            await using var _db = new ApplicationDbContext(_dbContext);
            _db.EmailLogs.Add(emaillLog);
            await _db.SaveChangesAsync();
        }
    }
}
