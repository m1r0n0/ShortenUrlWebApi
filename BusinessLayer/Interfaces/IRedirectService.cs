namespace BusinessLayer.Interfaces
{
    public interface IRedirectService
    {
        string GetLinkToRedirect(string shortUrl, string userName);
    }
}