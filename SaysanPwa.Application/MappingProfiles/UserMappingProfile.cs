using AutoMapper;
using SaysanPwa.Application.DTOs.User;
using SaysanPwa.Domain.AggregateModels.UserAggregate;

namespace SaysanPwa.Application.MappingProfiles;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>()
        .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.ID_tbl_Users))
        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
        .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State_User));

        //CreateMap<List<User>, List<UserDto>>();
    }
}