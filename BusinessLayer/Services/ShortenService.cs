using AutoMapper;
using BusinessLayer.DTOs;
using BusinessLayer.Exceptions;
using BusinessLayer.Helpers;
using BusinessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
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
            Url? linkWithSimilarFullUrlFromThisUser = await _context.UrlList.Where(l => l.FullUrl == modelDTO.FullUrl && l.UserId == modelDTO.UserId).FirstOrDefaultAsync();
            bool isThereSimilarFullUrl = linkWithSimilarFullUrlFromThisUser is not null;
            if (isThereSimilarFullUrl)
            {
                linkWithSimilarFullUrlFromThisUser.ShortUrl = ShortenHelper.AssembleShortUrl(linkWithSimilarFullUrlFromThisUser.ShortUrl, _configuration["shortenedBegining"]);
                return _mapper.Map<LinkDTO>(linkWithSimilarFullUrlFromThisUser);
            }
            string shortened = string.Empty;
            modelDTO.UserId ??= "";
            while (true)
            {
                Url? appropriateLink = await _context.UrlList.Where(x => x.ShortUrl.Equals(shortened))
                    .FirstOrDefaultAsync();
                bool isThereSimilarShortUrl = false;
                if (appropriateLink != null)
                {
                    if (shortened == appropriateLink.ShortUrl)
                    {
                        isThereSimilarShortUrl = true;
                        break;
                    }
                    else
                    {
                        isThereSimilarShortUrl = false;
                    }
                }
                else
                    break;

                if (!isThereSimilarShortUrl)
                {
                    break;
                }
            }

            Url urlObj = new()
            { UserId = modelDTO.UserId, FullUrl = modelDTO.FullUrl, IsPrivate = modelDTO.IsPrivate };
            _context.UrlList.Add(urlObj);
            await _context.SaveChangesAsync();
            urlObj.ShortUrl = IdToShortURL(urlObj.Id);
            await _context.SaveChangesAsync();
            modelDTO.ShortUrl = ShortenHelper.AssembleShortUrl(urlObj.ShortUrl, _configuration["shortenedBegining"]);
            return modelDTO;

        }

        public async Task<UrlListDTO> GetURLsForCurrentUser(string userId)
        {
            UrlListDTO tempList = new UrlListDTO();
            LinkDTO modelDTO = new();
            LinkForMyLinks link = new LinkForMyLinks();
            if (_context.UrlList != null)
            {
                modelDTO.UrlList = await _context.UrlList.Where(i => i.UserId == userId).ToListAsync();
                foreach (Url url in modelDTO.UrlList)
                {
                    url.ShortUrl = ShortenHelper.AssembleShortUrl(url.ShortUrl, _configuration["shortenedBegining"]);
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
            return new String(shorturl.ToCharArray().Reverse().ToArray()).ToString();
        }

        public int ShortURLToID(string shortUrl)
        {
            int id = 0;
            for (int i = 0; i < shortUrl.Length; i++)
            {
                if ('a' <= shortUrl[i] &&
                           shortUrl[i] <= 'z')
                    id = id * 62 + shortUrl[i] - 'a';
                if ('A' <= shortUrl[i] &&
                           shortUrl[i] <= 'Z')
                    id = id * 62 + shortUrl[i] - 'A' + 26;
                if ('0' <= shortUrl[i] &&
                           shortUrl[i] <= '9')
                    id = id * 62 + shortUrl[i] - '0' + 52;
            }
            return id;
        }

        public async Task ChangePrivacy(string shortUrl, bool state, string userId)
        {
            Url? link = await _context.UrlList.Where(x => x.ShortUrl == shortUrl).FirstOrDefaultAsync();
            if (link != null)
            {
                if (link.UserId != String.Empty)
                {
                    if (link.UserId == userId)
                    {
                        link.IsPrivate = !state;
                        await _context.SaveChangesAsync();
                    }
                }
                else throw new UnauthorizedException();

            }
            else throw new NotFoundException();
        }

        public async Task<Url> DeleteLink(Url url)
        {
            url.ShortUrl = url.ShortUrl.Split('/').ToList().Last();
            Url? link = await _context.UrlList.Where(u => u.ShortUrl == url.ShortUrl).FirstOrDefaultAsync();
            if (link != null)
            {
                if (link.UserId == url.UserId)
                {
                    _context.Entry(link).State = EntityState.Deleted;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new UnauthorizedException();
                }
            }
            else throw new NotFoundException();

            return link;
        }
    }
}
