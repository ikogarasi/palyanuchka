using AutoMapper;
using Restaurant.Services.ProductAPI.Models.Dto;
using Restaurant.Services.ProductAPI.Models.Dtos;

namespace Restaurant.Services.ProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDto, Product>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
