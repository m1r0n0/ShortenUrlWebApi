﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class LinkForMyLinks
    {
        public string FullUrl { get; set; } = string.Empty;
        public string ShortUrl { get; set; } = string.Empty;
        public bool IsPrivate { get; set; }
    }
}
