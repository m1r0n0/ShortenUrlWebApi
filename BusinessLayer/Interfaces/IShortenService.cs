using BusinessLayer.DTOs;
using DataAccessLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IShortenService
    {
        void ChangePrivacy(int id, bool state, string userId);
        Task<LinkDTO> CreateShortLinkFromFullUrl(LinkDTO modelDTO, string userId);
        LinkDTO GetURLsForCurrentUser(string userId);
        string IdToShortURL(int n);
        int ShortURLToID(string shortUrl);
    }
}
