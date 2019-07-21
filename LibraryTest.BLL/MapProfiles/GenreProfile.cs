using AutoMapper;
using LibraryTest.DAL.Entities;
using LibraryTest.Models.Genres;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryTest.BLL.MapProfiles
{
    public class GenreProfile : Profile
    {
        public GenreProfile()
        {
            CreateMap<GenreModel, Genre>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id ?? Guid.NewGuid()));

            CreateMap<Genre, GenreModel>();
        }
    }
}
