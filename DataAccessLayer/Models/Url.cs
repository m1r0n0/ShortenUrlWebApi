using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    [Index("UserId")]
    public class Url
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string FullUrl { get; set; } = string.Empty;
        public string ShortUrl { get; set; } = string.Empty;
        public bool IsPrivate { get; set; }
    }
}
