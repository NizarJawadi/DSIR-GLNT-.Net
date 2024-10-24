using AutoMapper;
using TP4.Dtos;
using TP4.Model;

namespace TP4.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie, MovieDetailsDto>();
            CreateMap<MovieDto, Movie>().ForMember(src => src.Poster, opt => opt.Ignore());
        }
    }
}
