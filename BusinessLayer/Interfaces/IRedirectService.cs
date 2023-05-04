using BusinessLayer.DTOs;

namespace BusinessLayer.Interfaces
{
    public interface IRedirectService
    {
        Task<string> GetFullUrlToRedirect(string shortUrl, string userId);
    }
}