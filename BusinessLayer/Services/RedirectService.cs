using BusinessLayer.Interfaces;
using DataAccessLayer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace BusinessLayer.Services
{
    public class RedirectService : IRedirectService
    {
        private readonly IShortenService _shortenService;
        private readonly ApplicationContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RedirectService(IShortenService shortenService, ApplicationContext applicationContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _shortenService = shortenService;
            _context = applicationContext;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetLinkToRedirect(string shortUrl, string userName)
        {
            string _fullUrl = string.Empty;
            string _checkHttp = string.Empty;
            if (shortUrl != null)
            {
                int _id = _shortenService.ShortURLToID(shortUrl);
                var _url = _context.UrlList.Where(x => x.Id.Equals(_id)).FirstOrDefault();

                if (_url != null)
                {
                    if (_url.IsPrivate)
                    {
                        if (_url.UserId == _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value)
                        {
                            _fullUrl = _url.FullUrl;
                        }
                        else
                        {
                            _fullUrl = "https://shorturl.com" + _configuration["port"] + "/Errors/PageNotFoundError";
                        }
                    }
                    else
                    {
                        _fullUrl = _url.FullUrl;
                    }
                }
                else
                {
                    _fullUrl = "https://shorturl.com" + _configuration["port"] + "/Errors/PageNotFoundError";
                }

                if (_fullUrl != string.Empty)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        _checkHttp += _fullUrl[i];
                    }
                    if ((_checkHttp != "http://") && (_checkHttp != "https:/"))
                    {
                        _fullUrl = "https://" + _fullUrl;
                    }
                }
                return _fullUrl;
            }
            else
            {
                return "https://shorturl.com" + _configuration["port"] + "/Home/Index";
            }
        }       
    }
}
