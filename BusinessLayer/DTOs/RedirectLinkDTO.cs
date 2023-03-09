namespace BusinessLayer.DTOs
{
    public class RedirectLinkDTO
    {
        public string ShortUrl { get; set; } = string.Empty;
        public string FullUrl { get; set; } = string.Empty;

        public RedirectLinkDTO(string shortUrl)
        {
            ShortUrl = shortUrl;
        }
    }
}
