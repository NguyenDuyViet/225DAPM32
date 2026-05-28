using AutoMapper;
using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.Models;

namespace Backend.Mapper
{
    public class FoodMapper : Profile
    {
        public FoodMapper()
        {
            // Request mapping
            CreateMap<FoodRequest, Food>();
            CreateMap<Food, FoodRequest>();
            
            // Response mapping
            CreateMap<Food, FoodResponse>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
                .ForMember(dest => dest.RestaurantName, opt => opt.MapFrom(src => src.Restaurant != null ? src.Restaurant.NameRestaurant : null))
                .ForMember(dest => dest.SoldQuantity, opt => opt.Ignore());
        }
    }
}
