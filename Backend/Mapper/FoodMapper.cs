using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Backend.Mapper
{
    public class FoodMapper : Profile
    {
        public FoodMapper()
        {
            CreateMap<DTOs.Request.FoodRequest, Models.Food>();
            CreateMap<Models.Food, DTOs.Request.FoodRequest>();
        }
    }
}
