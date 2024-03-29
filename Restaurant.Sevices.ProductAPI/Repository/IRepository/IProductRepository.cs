﻿using Restaurant.Services.ProductAPI.Models.Dtos;

namespace Restaurant.Services.ProductAPI.Repository.IRepository
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDto>> GetProducts();
        Task<ProductDto> GetProductById(int id);
        Task<ProductDto> CreateUpdateProduct(ProductDto product);
        Task<bool> DeleteProduct(int id);
    }
}
