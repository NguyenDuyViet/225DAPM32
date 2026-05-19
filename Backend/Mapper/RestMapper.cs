using AutoMapper;
using Backend.DTOs.Request;
using Backend.Models;

namespace Backend.Mapper
{
    public class RestMapper : Profile
    {
        public RestMapper()
        {
            CreateMap<RestRequestDTO, Restaurant>();
            CreateMap<Restaurant, RestRequestDTO>();
        }
    }
}
