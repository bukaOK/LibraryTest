using AutoMapper;
using LibraryTest.DAL.Entities;
using LibraryTest.DAL.Filters;
using LibraryTest.Models.Books;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryTest.BLL.MapProfiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<BookModel, Book>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id ?? Guid.NewGuid()));
            CreateMap<Book, BookModel>();

            CreateMap<Book, CatalogBookModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString("N")));

            CreateMap<BookFilterModel, BookListFilter>()
                .ForMember(dest => dest.GenreIds, opt => opt.MapFrom(src => src.Genres));

            CreateMap<BookMoveModel, BookMove>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id ?? Guid.NewGuid()));

            CreateMap<StoryModel, Story>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id ?? Guid.NewGuid()));
            CreateMap<Story, StoryModel>();
        }
    }
}
