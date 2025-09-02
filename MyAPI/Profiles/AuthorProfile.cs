using AutoMapper;
using MyAPI.Dtos;
using MyAPI.Extensions;
using MyAPI.Models;

namespace MyAPI.Profiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            // ทำจาก Models.Author ไปยัง AuthorDto
            CreateMap<Models.Author, AuthorDto>()
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => $"{src.FirstName},{src.LastName}"))

                .ForMember(
                    dest => dest.Age,
                    opt => opt.MapFrom(src => Util.GetCurrentAge(src.DateOfBirth)));


            CreateMap<Models.Author, Author2Dto>()
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => $"{src.LastName},{src.FirstName}"))

                .ForMember(
                    dest => dest.Age,
                    opt => opt.MapFrom(src => Util.GetCurrentAge(src.DateOfBirth)));

            // ทำจาก AuthorDto ไปยัง Models.Author
            CreateMap<AuthorCreateDto, Author>();
        }
    }

}
