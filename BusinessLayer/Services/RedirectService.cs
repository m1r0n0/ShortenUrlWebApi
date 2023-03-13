using BusinessLayer.Interfaces;
using DataAccessLayer.Data;
using Microsoft.AspNetCore.Http;
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
        public string GetFullUrlToRedirect(string shortUrl, string userId)
        {
            string fullUrl = string.Empty;
            string checkHttp = string.Empty;
            int id = _shortenService.ShortURLToID(shortUrl);
            var url = _context.UrlList.Where(x => x.Id.Equals(id)).FirstOrDefault();

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

                if (fullUrl != string.Empty)
                {
                    if (!(fullUrl.StartsWith("http://") || fullUrl.StartsWith("https://"))) fullUrl = "https://" + fullUrl;
                }
            }
        }
    }
}
