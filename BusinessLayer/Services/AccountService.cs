using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class AccountService : IAccountService
    {
        private readonly DataAccessLayer.Data.ApplicationContext _context;
        public AccountService(DataAccessLayer.Data.ApplicationContext context)
        {
            _context = context;
        }
        public UserLoginInfoDTO GetUserEmailAndLoginStatus(string userId, bool isLogon)
        {
            UserLoginInfoDTO userLoginInfoDTO = new(isLogon);
            userLoginInfoDTO.UserEmail = _context.UserList?.Where(item => item.Id == userId)?.FirstOrDefault()?.ToString();
            if (userLoginInfoDTO.UserEmail == null)
            {
                userLoginInfoDTO.UserEmail = "";
            }
            return userLoginInfoDTO;
        }
    }
}
