using AutoMapper;
using BusinessLayer.DTOs;
using DataAccessLayer.Models;

namespace ShortenUrlWebApi.MappingProfiles
{
    public class AppMappingProfileForLinkForMyLinks : Profile
    {
        public AppMappingProfileForLinkForMyLinks()
        {
            CreateMap<LinkForMyLinks, Url>().ReverseMap();
        }
    }
}
