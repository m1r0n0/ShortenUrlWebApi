using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
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
                User user = new User { Email = model.Email, UserName = model.Email, Year = model.Year };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return Ok(model);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
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
                return Ok(model);
            }
            else
            {
                ModelState.AddModelError("", "Incorrect login and (or) password");
            }
            return BadRequest(model);
        }

        [HttpGet]
        public UserEmailIdDTO GetUserID(string userEmail)
        {
            return _accountService.GetUserIDFromUserEmail(userEmail);
        }

        [HttpGet]
        public UserEmailIdDTO GetUserEmail(string userID)
        {
            return _accountService.GetUserEmailFromUserID(userID);
        }

        [HttpGet]
        public CheckExistingEmailDTO CheckEmailExisting(string email)
        {
            CheckExistingEmailDTO model = new(email);
            model.IsExist = _accountService.CheckGivenEmailForExistingInDB(email);
            return model;
        }
    }
}