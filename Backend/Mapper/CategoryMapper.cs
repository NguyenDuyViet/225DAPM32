using AutoMapper;
using Backend.DTOs.Request;
using Backend.Models;

namespace Backend.Mapper
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper()
        {

            // Request mapping
            CreateMap<CategoryRequest, Category>();
            CreateMap<Category, CategoryRequest>();
            
            // Response mapping
            CreateMap<Category, CategoryResponse>()
                .ForMember(dest => dest.FoodCount, opt => opt.MapFrom(src => src.Foods != null ? src.Foods.Count : 0));
        }
    }
}
