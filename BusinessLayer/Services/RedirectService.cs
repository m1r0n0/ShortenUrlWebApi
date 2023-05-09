using BusinessLayer.Interfaces;
using DataAccessLayer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BusinessLayer.Services
{
    public class RedirectService : IRedirectService
    {
        private readonly IShortenService _shortenService;
        private readonly ApplicationContext _context;
        private readonly IConfiguration _configuration;

        public RedirectService(IShortenService shortenService, ApplicationContext applicationContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _shortenService = shortenService;
            _context = applicationContext;
            _configuration = configuration;
        }
        public async Task<string> GetFullUrlToRedirect(string shortUrl, string userId)
        {
            string fullUrl;
            int id = _shortenService.ShortURLToID(shortUrl);
            var url = await _context.UrlList.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();

            if (shortUrl != null)
            {
                GetFullUrlFromShorten();
                PrepareFullUrlToRedirectInWWW();
            }
            else
            {
                fullUrl = _configuration["shortenedBegining"] + _configuration["port"] + "/";
            }
            return fullUrl;


            void GetFullUrlFromShorten()
            {
                if (url != null)
                {
                    if (url.IsPrivate)
                    {
                        if (url.UserId == userId)
                        {
                            fullUrl = url.FullUrl;
                        }
                        else
                        {
                            fullUrl = _configuration["shortenedBegining"] + _configuration["port"] + "/Unauthorized";
                        }
                    }
                    else
                    {
                        fullUrl = url.FullUrl;
                    }
                }
                else
                {
                    fullUrl = _configuration["shortenedBegining"] + _configuration["port"] + "/NotFound";
                }
            }

            void PrepareFullUrlToRedirectInWWW()
            {
                if (fullUrl == string.Empty) return;
                if (!(fullUrl.StartsWith("http://") || fullUrl.StartsWith("https://"))) fullUrl = "https://" + fullUrl;
            }
        }
    }
}
