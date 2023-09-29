using AutoMapper;
using Restaurant.Service.OrderAPI.Models;
using Restaurant.Service.OrderAPI.Models.Dto;

namespace Restaurant.Service.OrderAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();
                config.CreateMap<OrderDetails, OrderDetailsDto>().ReverseMap();
            });
        }
    }
}
