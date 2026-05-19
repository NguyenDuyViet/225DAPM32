using AutoMapper;
using Backend.DTOs.Request;
using Backend.Models;

namespace Backend.Mapper
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper()
        {
            CreateMap<CategoryRequest, Category>();
            CreateMap<Category, CategoryRequest>();
        }
    }
}
