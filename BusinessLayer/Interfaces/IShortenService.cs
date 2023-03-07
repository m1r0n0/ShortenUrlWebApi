using BusinessLayer.DTOs;

namespace BusinessLayer.Interfaces
{
    public interface IShortenService
    {
        void ChangePrivacy(string shortUrl, bool state, string userId);
        Task<LinkDTO> CreateShortLinkFromFullUrl(LinkDTO modelDTO);
        UrlListDTO GetURLs();
        UrlListDTO GetURLsForCurrentUser(string userId);
        string IdToShortURL(int n);
        int ShortURLToID(string shortUrl);
    }
}
