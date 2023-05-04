using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services
{
    public class AccountService : IAccountService
    {
        private readonly DataAccessLayer.Data.ApplicationContext _context;
        public AccountService(DataAccessLayer.Data.ApplicationContext context)
        {
            _context = context;
        }
        public async Task<UserEmailIdDTO> GetUserIDFromUserEmail(string userEmail)
        {
            UserEmailIdDTO userEmailIdDTO = new()
            {
                NewEmail = userEmail
            };
            User? tempUserEmailToIdDTO = await _context.UserList?.Where(item => item.Email == userEmail)?.FirstOrDefaultAsync()!;
            userEmailIdDTO.UserId = tempUserEmailToIdDTO == null ? "" : tempUserEmailToIdDTO.Id;
            return userEmailIdDTO;
        }

        public async Task<UserEmailIdDTO> GetUserEmailFromUserID(string userId)
        {
            UserEmailIdDTO userEmailIdDTO = new()
            {
                UserId = userId
            };
            User? tempUser = await _context.UserList?.Where(item => item.Id == userId)?.FirstOrDefaultAsync()!;
            userEmailIdDTO.NewEmail = tempUser == null ? "" : tempUser.Email;
            return userEmailIdDTO;
        }

        public async Task<bool> CheckGivenEmailForExistingInDB(string email)
        {
            bool isEmailExists = await _context.UserList?.AnyAsync(item => item.Email == email)!;
            return isEmailExists;
        }

        public async Task<UserEmailIdDTO> setNewUserEmail(string newUserEmail, string userID)
        {
            UserEmailIdDTO userEmailIdDTO = new(userID);
            var userToPatch = await _context.UserList?.Where(user => user.Id == userID).FirstOrDefaultAsync()!;
            var probablyExistingUser = await _context.UserList?.Where(user => user.Email == newUserEmail).FirstOrDefaultAsync()!;
            if (probablyExistingUser == null)
            {
                UpdateUserInDB();
                userEmailIdDTO.NewEmail = newUserEmail;
            }
            else
            {
                userEmailIdDTO.NewEmail = null;
            }
            return userEmailIdDTO;

            void UpdateUserInDB()
            {
                userToPatch.Email = newUserEmail;
                userToPatch.NormalizedEmail = newUserEmail.ToUpper();
                userToPatch.UserName = newUserEmail;
                userToPatch.NormalizedUserName = newUserEmail.ToUpper();

                userToPatch = null;

                _context.SaveChanges();
            }
        }

        public User? GetUserById(string Id)
        {
            return _context.UserList?.Where(user => user.Id == Id).FirstOrDefault();
        }
    }
}
