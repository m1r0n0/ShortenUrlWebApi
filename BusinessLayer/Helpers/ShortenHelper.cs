using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Helpers
{
    internal class ShortenHelper
    {
        public static string AssembleShortUrl (string shortPartOfUrl, string shortenedBegining)
        {
            return shortenedBegining + "/" + shortPartOfUrl;
        }
    }
}
