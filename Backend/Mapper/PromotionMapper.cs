using AutoMapper;
using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.Models;

namespace Backend.Mapper
{
    public class PromotionMapper : Profile
    {
        public PromotionMapper()
        {
            CreateMap<PromotionRequest, Promotion>();
            CreateMap<Promotion, PromotionRequest>();
            CreateMap<Promotion, PromotionResponse>();
        }
    }
}
