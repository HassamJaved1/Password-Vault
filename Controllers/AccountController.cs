using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Password_Vault.Data;
using Password_Vault.Models;
using Password_Vault.Services;
using System.Security.Claims;


namespace Password_Vault.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly IAdminRepository _admins;
        private readonly IUserRepository _users;
        private readonly IWhitelistRepository _whitelist;
        private readonly IAuthService _auth;

        public AccountController(IAdminRepository admins, IUserRepository users, IWhitelistRepository whitelist, IAuthService auth)
        {
            _admins = admins;
            _users = users;
            _whitelist = whitelist;
            _auth = auth;
        }

        [HttpGet]
        public IActionResult Login() => View(new LoginViewModel());

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.UsernameOrUserId) || string.IsNullOrWhiteSpace(vm.Password))
            {
                ModelState.AddModelError("", "Username/User ID and password are required.");
                return View(vm);
            }

            // Try admin first
            var admin = await _admins.GetByUsernameAsync(vm.UsernameOrUserId);
            if (admin is not null && _auth.VerifyPassword(vm.Password, admin.Password_Hash))
            {
                await SignInAsync("admin", admin.Username);
                return RedirectToAction("Index", "Home");
            }

            // Then user by user_id
            var user = await _users.GetByUserIdAsync(vm.UsernameOrUserId);
            if (user is not null && _auth.VerifyPassword(vm.Password, user.Password_Hash))
            {
                await SignInAsync("user", user.User_Id);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid credentials.");
            return View(vm);
        }

        [HttpGet]
        public IActionResult Register() => View(new RegisterViewModel());

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!_auth.ValidateUserIdFormat(vm.UserId))
                ModelState.AddModelError(nameof(vm.UserId), "User ID must be like ACE-XXXX.");

            if (string.IsNullOrWhiteSpace(vm.Password) || vm.Password.Length < 8)
                ModelState.AddModelError(nameof(vm.Password), "Password must be at least 8 characters.");

            if (vm.Password != vm.ConfirmPassword)
                ModelState.AddModelError(nameof(vm.ConfirmPassword), "Passwords do not match.");

            // Must be whitelisted and not revoked
            var whitelisted = await _whitelist.IsWhitelistedAsync(vm.UserId);
            if (!whitelisted)
                ModelState.AddModelError(nameof(vm.UserId), "This User ID is not whitelisted or access has been revoked.");

            if (!ModelState.IsValid)
                return View(vm);

            var existingUser = await _users.GetByUserIdAsync(vm.UserId);
            if (existingUser is not null)
            {
                ModelState.AddModelError(nameof(vm.UserId), "This User ID is already registered.");
                return View(vm);
            }

            var hash = _auth.HashPassword(vm.Password);
            await _users.CreateAsync(vm.UserId.ToUpperInvariant(), hash);

            TempData["msg"] = "Account created. Please log in.";
            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction(nameof(Login));
        }

        private async Task SignInAsync(string role, string name)
        {
            var claims = new List<Claim>
            {
               new(ClaimTypes.Name, name),
               new(ClaimTypes.Role, role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
        }
    }

}
