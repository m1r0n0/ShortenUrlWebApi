using AutoMapper;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ShortenUrlWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RedirectController : AppController
    {
        private readonly IRedirectService _redirectService;

        public RedirectController(IHttpContextAccessor httpContextAccessor, IRedirectService redirectService, IMapper mapper) : base(httpContextAccessor)
        {
            _redirectService = redirectService;
        }

        [HttpGet]
        public async Task<IActionResult> RedirectToOriginalUrl(string shortUrl, string? userId)
        {
            return Redirect(await _redirectService.GetFullUrlToRedirect(shortUrl, userId));
        }
    }
}
