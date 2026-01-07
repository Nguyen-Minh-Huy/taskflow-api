using AutoMapper;
using TaskFlow.Application.DTOs.Auth;
using TaskFlow.Domain.Entities;
namespace TaskFlow.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<RegisterRequestDto, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.SystemRole, opt => opt.MapFrom(src => "User"));
    }
}