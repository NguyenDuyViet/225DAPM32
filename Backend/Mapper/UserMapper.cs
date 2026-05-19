using AutoMapper;
using Backend.DTOs.Response;
using Backend.DTOs.Request;
using Backend.Models;

namespace Backend.Mapper
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.IdRole, opt => opt.MapFrom(src => src.IdRole ?? 2));
            CreateMap<UpdateUserDto, User>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<User, UserResponse>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.RoleName : null));
        }
    }
}
