using AutoMapper;
using BusinessLayer.DTOs;
using DataAccessLayer.Models;

namespace ShortenUrlWebApi.MappingProfiles
{
    public class AppMappingProfileForUrlList : Profile
    {
        public AppMappingProfileForUrlList()
        {
            CreateMap<LinkDTO, UrlListDTO>().ReverseMap();
        }
    }
}
