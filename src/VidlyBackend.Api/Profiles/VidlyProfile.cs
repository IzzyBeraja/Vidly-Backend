using Authenticator.Models;
using AutoMapper;
using VidlyBackend.Dto;
using VidlyBackend.Models;

namespace VidlyBackend.Profiles
{
    public class VidlyProfile : Profile
    {
        public VidlyProfile()
        {
            // Source -> Destination
            // Movies
            CreateMap<Movie, MovieReadDto>();
            CreateMap<MovieCreateDto, Movie>();
            CreateMap<MovieUpdateDto, Movie>();
            CreateMap<Movie, MovieUpdateDto>();
            // Genres
            CreateMap<Genre, GenreReadDto>();
            CreateMap<GenreCreateDto, Genre>();
            CreateMap<GenreUpdateDto, Genre>();
            CreateMap<Genre, GenreUpdateDto>();
            // Users
            CreateMap<User, UserReadDto>();
            CreateMap<UserCreateDto, User>();
            CreateMap<User, TokenModel>();
            CreateMap<UserReadDto, TokenModel>();
        }
    }
}
