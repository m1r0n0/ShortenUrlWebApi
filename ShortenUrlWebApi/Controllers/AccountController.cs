using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ShortenUrlWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : AppController
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IAccountService _accountService;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IHttpContextAccessor httpContextAccessor,
            IAccountService accountService
            ) : base(httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _accountService = accountService;
        }

        [HttpPut]
        public async Task<IActionResult> Register(UserDTO model)
        {
            if (ModelState.IsValid)
            {
                if (await _accountService.CheckGivenEmailForExistingInDB(model.Email))
                {
                    return Conflict(model);
                }
                User user = new User { Email = model.Email, UserName = model.Email, Year = model.Year };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    UserEmailIdDTO emailIdDTO = await _accountService.GetUserIDFromUserEmail(model.Email);
                    model.UserId = emailIdDTO.UserId;
                    await _signInManager.SignInAsync(user, false);
                    return Ok(model);
                }
            }
            return BadRequest(model);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> Login([FromBody] UserDTO model)
        {
            var result =
                await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                UserEmailIdDTO emailIdDTO = await _accountService.GetUserIDFromUserEmail(model.Email);
                model.UserId = emailIdDTO.UserId;
                return Ok(model);
            }
            return BadRequest(model);
        }

        [HttpGet]
        public async Task<UserEmailIdDTO> GetUserID(string userEmail)
        {
            return await _accountService.GetUserIDFromUserEmail(userEmail);
        }

        [HttpGet]
        public async Task<UserEmailIdDTO> GetUserEmail(string userID)
        {
            return await _accountService.GetUserEmailFromUserID(userID);
        }

        [HttpGet]
        public async Task<CheckExistingEmailDTO> CheckEmailExisting(string email)
        {
            return new(email, await _accountService.CheckGivenEmailForExistingInDB(email));
        }

        [HttpPatch]
        public async Task<UserEmailIdDTO> ChangeUserEmail(UserEmailIdDTO model)
        {
            return await _accountService.setNewUserEmail(model.NewEmail!, model.UserId);
        }

        [HttpPatch]
        public async Task<IActionResult> ChangeUserPassword(UserPasswordIdDTO model)
        {
            User user = _accountService.GetUserById(model.UserId)!;
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
            if (result.Succeeded)
            {
                return Ok(model);
            }
            else
            {
                return BadRequest(model);
            }
        }
    }
}