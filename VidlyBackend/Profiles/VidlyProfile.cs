using AutoMapper;
using VidlyBackend.Dto;
using VidlyBackend.Models;

namespace VidlyBackend.Profiles
{
    public class VidlyProfile : Profile
    {
        public VidlyProfile()
        {
            CreateMap<Movie, MovieReadDto>();
        }
    }
}
