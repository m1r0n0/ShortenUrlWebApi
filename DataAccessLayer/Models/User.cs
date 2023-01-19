using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models
{
    [Index("UserName")]
    public class User : IdentityUser
    {
        public int Year { get; set; }
    }
}
