using AutoMapper;
using MyAPI.Dtos;
using MyAPI.Models;

namespace MyAPI.Profiles
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseDto>();

            CreateMap<CourseCreateDto, Course>();
        }

    }
}
