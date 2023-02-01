using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class UrlListDTO
    {
        public IList<LinkForMyLinks> UrlList { get; set; } = default!;

        public UrlListDTO ()
        {
            UrlList = new List<LinkForMyLinks> ();
        }
    }
}
