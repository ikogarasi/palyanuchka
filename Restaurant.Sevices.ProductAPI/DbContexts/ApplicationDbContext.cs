using Microsoft.EntityFrameworkCore;
using Restaurant.Sevices.ProductAPI.Models.Dto;

namespace Restaurant.Sevices.ProductAPI.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 1,
                ProductName = "Kanapka",
                Price = 15,
                ProductDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque eleifend dui nunc, vel aliquet diam viverra vel. ",
                ImageURL = "https://vitaliikutsan.blob.core.windows.net/palyanuchka/Images/kanapka.jpg",
                CategoryName = "Appetizer"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 2,
                ProductName = "Palyanuchka",
                Price = 18,
                ProductDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque eleifend dui nunc, vel aliquet diam viverra vel. ",
                ImageURL = "https://vitaliikutsan.blob.core.windows.net/palyanuchka/Images/palyanychka.jpg",
                CategoryName = "Appetizer"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 3,
                ProductName = "Sushiky",
                Price = 18,
                ProductDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque eleifend dui nunc, vel aliquet diam viverra vel. ",
                ImageURL = "https://vitaliikutsan.blob.core.windows.net/palyanuchka/Images/sushiki.jpg",
                CategoryName = "Sushi"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 4,
                ProductName = "Varenyky",
                Price = 18,
                ProductDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque eleifend dui nunc, vel aliquet diam viverra vel. ",
                ImageURL = "https://vitaliikutsan.blob.core.windows.net/palyanuchka/Images/varenyky.jpg",
                CategoryName = "Dish"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 5,
                ProductName = "Wings",
                Price = 18,
                ProductDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque eleifend dui nunc, vel aliquet diam viverra vel. ",
                ImageURL = "https://vitaliikutsan.blob.core.windows.net/palyanuchka/Images/wings.jpg",
                CategoryName = "Dish"
            });
        }
    }
}
