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
        public string GetUserEmailUsingUserId(string userId)
        {
            string userEmail = _context.UserList?.Where(item => item.Id == userId).FirstOrDefault().ToString();
            if (userEmail == null)
            {
                userEmail = "";
            }
            return userEmail;
        }
    }
}
