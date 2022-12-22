using RestaurantWeb.Models;

namespace RestaurantWeb.Services.IServices
{
    public interface IProductService : IBaseService
    {
        Task<T> GetAllProductsAsync<T>();
        Task<T> GetProductByIdAsync<T>(int id);
        Task<T> CreateProductAsync<T>(ProductDto productDto, string accessToken);
        Task<T> UpdateProductAsync<T>(ProductDto productDto, string accessToken);
        Task<T> DeleteProductAsync<T>(int id, string accessToken);

    }
}
