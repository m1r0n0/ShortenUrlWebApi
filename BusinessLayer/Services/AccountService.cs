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
        public UserEmailIdDTO GetUserIDFromUserEmail(string userEmail)
        {
            UserEmailIdDTO userEmailIdDTO = new();
            userEmailIdDTO.UserEmail = userEmail;
            var tempUserEmailToIdDTO = _context.UserList?.Where(item => item.Email == userEmail)?.FirstOrDefault();
            if (tempUserEmailToIdDTO == null)
            {
                userEmailIdDTO.UserID = "";
            }
            else
            { 
                userEmailIdDTO.UserID = tempUserEmailToIdDTO.Id; 
            }
            return userEmailIdDTO;
        }

        public UserEmailIdDTO GetUserEmailFromUserID(string userId)
        {
            UserEmailIdDTO userEmailIdDTO = new();
            userEmailIdDTO.UserID = userId;
            var tempUserEmailToIdDTO = _context.UserList?.Where(item => item.Id == userId)?.FirstOrDefault();
            if (tempUserEmailToIdDTO == null)
            {
                userEmailIdDTO.UserEmail = "";
            }
            else
            {
                userEmailIdDTO.UserEmail = tempUserEmailToIdDTO.Email;
            }
            return userEmailIdDTO;
        }
    }
}
