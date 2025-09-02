using AutoMapper;
using MyAPI.Dtos;
using MyAPI.Extensions;
using MyAPI.Models;

namespace MyAPI.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserCreateDto, User>()
              .ForMember(
                    dest => dest.Password,
                    opt => opt.MapFrom(src => Util.ComputeMD5Hash(src.Password))
                    );
        }
    }
}
