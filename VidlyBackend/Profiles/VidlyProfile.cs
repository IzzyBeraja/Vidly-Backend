﻿using AutoMapper;
using VidlyBackend.Dto;
using VidlyBackend.Models;

namespace VidlyBackend.Profiles
{
    public class VidlyProfile : Profile
    {
        public VidlyProfile()
        {
            // Source -> Destination
            CreateMap<Movie, MovieReadDto>();
            CreateMap<MovieCreateDto, Movie>();
            CreateMap<MovieUpdateDto, Movie>();
            CreateMap<Genre, GenreReadDto>();
            CreateMap<GenreCreateDto, Genre>();
        }
    }
}
