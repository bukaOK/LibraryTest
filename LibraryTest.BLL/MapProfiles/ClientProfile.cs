using AutoMapper;
using LibraryTest.DAL.Entities;
using LibraryTest.Models.Clients;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryTest.BLL.MapProfiles
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<ClientModel, Client>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id ?? Guid.NewGuid()));

            CreateMap<Client, ClientModel>();
        }
    }
}
