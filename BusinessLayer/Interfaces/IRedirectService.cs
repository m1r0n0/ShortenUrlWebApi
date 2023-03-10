using BusinessLayer.DTOs;

namespace BusinessLayer.Interfaces
{
    public interface IRedirectService
    {
        string GetFullUrlToRedirect(string shortUrl, string userId);
    }
}