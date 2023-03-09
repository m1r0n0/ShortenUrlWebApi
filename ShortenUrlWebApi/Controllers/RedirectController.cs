using AutoMapper;
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ShortenUrlWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RedirectController : AppController
    {
        private readonly IRedirectService _redirectService;
        private readonly IMapper _mapper;

        public RedirectController(IHttpContextAccessor httpContextAccessor, IRedirectService redirectService, IMapper mapper) : base(httpContextAccessor)
        {
            _redirectService = redirectService;
            _mapper = mapper;
        }

        [HttpGet]
        public RedirectLinkDTO GetFullUrlToRedirect(string shortUrl, string? userId)
        {
            return _redirectService.GetFullUrlToRedirect(shortUrl, userId);
        }
    }
}
