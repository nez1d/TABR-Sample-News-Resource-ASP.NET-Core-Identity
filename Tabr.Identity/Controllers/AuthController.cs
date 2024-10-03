using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tabr.Identity.Entities.User;
using Tabr.Identity.Models;

namespace Tabr.Identity.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IIdentityServerInteractionService _interactionService;

        public AuthController(SignInManager<AppUser> signInManager, 
            UserManager<AppUser> userManager,
            IIdentityServerInteractionService interactionService) =>
            (_signInManager, _userManager, _interactionService) = 
            (signInManager, userManager, interactionService);
        

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            var viewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByLoginAsync(model.Login, model.Login);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User not found");
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(
                model.Login,
                model.Password,
                false, false);
            if(result.Succeeded)
            {
                return Redirect(model.ReturnUrl);
            }
            ModelState.AddModelError(string.Empty, "Login error");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            var viewModel = new RegisterViewModel
            {
                ReturnUrl = returnUrl
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new AppUser
            {
                Login = model.Login
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return Redirect(model.ReturnUrl);
            }
            ModelState.AddModelError(string.Empty, "Error occurred");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string returnUrl)
        {
            await _signInManager.SignOutAsync();
            var logoutRequest = await _interactionService
                .GetLogoutContextAsync(returnUrl);
            return Redirect(logoutRequest.PostLogoutRedirectUri);
        }
    }
}