using BusinessLayer.DTOs;
using DataAccessLayer.Models;

namespace BusinessLayer.Interfaces
{
    public interface IAccountService
    {
        Task<UserEmailIdDTO> GetUserIDFromUserEmail(string userEmail);
        Task<UserEmailIdDTO> GetUserEmailFromUserID(string userID);
        Task<bool> CheckGivenEmailForExistingInDB(string email);
        Task<UserEmailIdDTO> setNewUserEmail(string newUserEmail, string userID);
        User? GetUserById(string Id);
    }
}
