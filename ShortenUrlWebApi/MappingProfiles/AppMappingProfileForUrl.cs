using AutoMapper;
using BusinessLayer.DTOs;
using DataAccessLayer.Models;

namespace ShortenUrlWebApi.MappingProfiles
{
    public class AppMappingProfileForUrl : Profile
    {
        public AppMappingProfileForUrl()
        {
            CreateMap<LinkDTO, Url>().ReverseMap();
        }
    }
}
