using AutoMapper;
using Fahem.Dtos.OrdersDtos;
using Fahem.Models;

namespace Fahem.Utils.AutoMapperProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<CreateOrderDto, Order>(); 
        }
    }

}
