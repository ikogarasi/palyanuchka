using System.ComponentModel.DataAnnotations;

namespace Restaurant.Services.ProductAPI.Models.Dto
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Range(1, 1000)]
        public double Price { get; set; }
        public string ProductDescription { get; set; }
        public string CategoryName { get; set; }
        public string ImageURL { get; set; }
    }
}
