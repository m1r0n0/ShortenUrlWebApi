using BusinessLayer.DTOs;

namespace BusinessLayer.Interfaces
{
    public interface IRedirectService
    {
        RedirectLinkDTO GetFullUrlToRedirect(string shortUrl, string userId);
    }
}