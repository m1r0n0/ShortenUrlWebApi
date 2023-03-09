using BusinessLayer.DTOs;
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
        public RedirectLinkDTO GetFullUrlToRedirect(string shortUrl, string userId)
        {
            RedirectLinkDTO model = new(shortUrl);
            string _checkHttp = string.Empty;
            int _id = _shortenService.ShortURLToID(shortUrl);
            var _url = _context.UrlList.Where(x => x.Id.Equals(_id)).FirstOrDefault();

            if (shortUrl != null)
            {
                GetFullUrlFromShorten();
                PrepareFullUrlToRedirectInWWW();
            }
            else
            {
                model.FullUrl = "https://shorturl.com" + _configuration["port"] + "/";
            }
            return model;


            void GetFullUrlFromShorten()
            {
                if (_url != null)
                {
                    if (_url.IsPrivate)
                    {
                        if (_url.UserId == userId)
                        {
                            model.FullUrl = _url.FullUrl;
                        }
                        else
                        {
                            model.FullUrl = "https://shorturl.com" + _configuration["port"] + "/Error/Unauthorized";
                        }
                    }
                    else
                    {
                        model.FullUrl = _url.FullUrl;
                    }
                }
                else
                {
                    model.FullUrl = "https://shorturl.com" + _configuration["port"] + "/Error/NotFound";
                }
            }

            void PrepareFullUrlToRedirectInWWW()
            {
                if (model.FullUrl != string.Empty)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        _checkHttp += model.FullUrl[i];
                    }
                    if ((_checkHttp != "http://") && (_checkHttp != "https:/"))
                    {
                        model.FullUrl = "https://" + model.FullUrl;
                    }
                }
            }
        }
    }
}
