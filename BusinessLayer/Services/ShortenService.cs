using AutoMapper;
using BusinessLayer.DTOs;
using BusinessLayer.Exceptions;
using BusinessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.Extensions.Configuration;

namespace BusinessLayer.Services
{
    public class ShortenService : IShortenService
    {
        private readonly IConfiguration _configuration;
        private readonly DataAccessLayer.Data.ApplicationContext _context;
        private readonly IMapper _mapper;

        public ShortenService(
            DataAccessLayer.Data.ApplicationContext context,
            IConfiguration configuration,
            IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<LinkDTO> CreateShortLinkFromFullUrl(LinkDTO modelDTO)
        {
            string _shortened = string.Empty;
            bool _isThereSimilar = false;
            if (modelDTO.UserId == null)
            {
                modelDTO.UserId = "";
            }
            while (true)
            {
                var appropriateShortLink = _context.UrlList.Where(x => x.ShortUrl.Equals(_shortened)).FirstOrDefault();
                if (appropriateShortLink != null)
                {
                    if (_shortened == appropriateShortLink.ShortUrl)
                    {
                        _isThereSimilar = true;
                        break;
                    }
                    else
                    {
                        _isThereSimilar = false;
                    }
                }
                else
                    break;

                if (!_isThereSimilar)
                {
                    break;
                }
            }
            Url urlObj = new() { UserId = modelDTO.UserId, FullUrl = modelDTO.FullUrl, IsPrivate = modelDTO.IsPrivate };
            _context.UrlList.Add(urlObj);
            await _context.SaveChangesAsync();
            urlObj.ShortUrl = IdToShortURL(urlObj.Id);
            await _context.SaveChangesAsync();
            modelDTO.ShortUrl = _configuration["shortenedBegining"] + "/" + urlObj.ShortUrl;
            return modelDTO;
        }

        public UrlListDTO GetURLsForCurrentUser(string userId)
        {
            UrlListDTO tempList = new UrlListDTO();
            LinkDTO modelDTO = new();
            LinkForMyLinks link = new LinkForMyLinks();
            if (_context.UrlList != null)
            {
                modelDTO.UrlList = _context.UrlList.Where(i => i.UserId == userId).ToList();
                foreach (Url url in modelDTO.UrlList)
                {
                    url.ShortUrl = _configuration["shortenedBegining"] + "/" + url.ShortUrl;
                }

                for (int i = 0; i < modelDTO.UrlList.Count(); i++)
                {
                    link = _mapper.Map<LinkForMyLinks>(modelDTO.UrlList.ElementAt(i));
                    tempList.UrlList.Add(link);
                }
            }
            return tempList;
        }

        public string? IdToShortURL(int n)
        {
            char[] map = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
            string shorturl = "";
            while (n > 0)
            {
                shorturl += (map[n % 62]);
                n /= 62;
            }
            return new String(shorturl.ToCharArray().Reverse().ToArray()).ToString(); ;
        }

        public int ShortURLToID(string shortURL)
        {
            int id = 0;
            for (int i = 0; i < shortURL.Length; i++)
            {
                if ('a' <= shortURL[i] &&
                           shortURL[i] <= 'z')
                    id = id * 62 + shortURL[i] - 'a';
                if ('A' <= shortURL[i] &&
                           shortURL[i] <= 'Z')
                    id = id * 62 + shortURL[i] - 'A' + 26;
                if ('0' <= shortURL[i] &&
                           shortURL[i] <= '9')
                    id = id * 62 + shortURL[i] - '0' + 52;
            }
            return id;
        }

        public void ChangePrivacy(string shortUrl, bool state, string userId)
        {
            var link = _context.UrlList.Where(x => x.ShortUrl == shortUrl).FirstOrDefault();
            if (link != null)
            {
                if (link.UserId != String.Empty)
                {
                    if (link.UserId == userId)
                    {
                        if (state)
                        {
                            link.IsPrivate = false;
                        }
                        else
                        {
                            link.IsPrivate = true;
                        }
                        _context.SaveChanges();
                    }
                }
                else throw new UnauthorizedException();

            }
            else throw new NotFoundException();
        }
    }
}
