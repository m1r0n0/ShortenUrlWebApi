using BusinessLayer.DTOs;

namespace BusinessLayer.Interfaces
{
    public interface IShortenService
    {
        Task ChangePrivacy(string shortUrl, bool state, string userId);
        Task<LinkDTO> CreateShortLinkFromFullUrl(LinkDTO modelDTO);
        Task<UrlListDTO> GetURLsForCurrentUser(string userId);
        string IdToShortURL(int n);
        int ShortURLToID(string shortUrl);
    }
}
