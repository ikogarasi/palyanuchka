using Microsoft.EntityFrameworkCore;
using Restaurant.Service.Email.Models;

namespace Restaurant.Service.Email.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<EmailLog> EmailLogs { get; set; }
    }
}
