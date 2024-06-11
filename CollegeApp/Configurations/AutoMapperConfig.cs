using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Models;

namespace CollegeApp.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            //configuration for different name properties
            //CreateMap<Student, StudentDTO>().ReverseMap().ForMember(x=>x.StudentName, opt=>opt.MapFrom(x=>x.Name));
            //configuration for ignoring some properties
            //CreateMap<Student, StudentDTO>().ReverseMap().ForMember(x => x.Name,opt=>opt.Ignore());
            //configuration for transforming some properties
            CreateMap<Student, StudentDTO>().ReverseMap()
                .ForMember(x => x.Address, opt => opt.MapFrom(n => string.IsNullOrEmpty(n.Address) ? "No Address" : n.Address));
            CreateMap<Student, StudentDTO>().ReverseMap();

        }
    }
}
