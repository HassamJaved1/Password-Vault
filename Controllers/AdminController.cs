using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Password_Vault.Data;
using Password_Vault.Models;
using System.Security.Claims;

namespace Password_Vault.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IWhitelistRepository _whitelist;
        private readonly IAdminRepository _admins;

        public AdminController(IWhitelistRepository whitelist, IAdminRepository admins)
        {
            _whitelist = whitelist;
            _admins = admins;
        }

        [HttpPost]
        public async Task<IActionResult> WhitelistUser([FromBody] WhitelistUserViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.UserId))
            {
                return Json(new { success = false, message = "User ID is required." });
            }

            if (!vm.ConfirmWhitelist)
            {
                return Json(new { success = false, message = "You must confirm that you want to whitelist this user." });
            }

            // Get admin ID from current user
            var adminUsername = User.Identity?.Name;
            if (string.IsNullOrEmpty(adminUsername))
            {
                return Json(new { success = false, message = "Admin session not found." });
            }

            var admin = await _admins.GetByUsernameAsync(adminUsername);
            if (admin == null)
            {
                return Json(new { success = false, message = "Admin not found." });
            }

            // Check if user is already whitelisted
            var isAlreadyWhitelisted = await _whitelist.IsWhitelistedAsync(vm.UserId);
            if (isAlreadyWhitelisted)
            {
                return Json(new { success = false, message = "This user is already whitelisted." });
            }

            // Add to whitelist
            var success = await _whitelist.AddToWhitelistAsync(admin.Id, vm.UserId);
            if (success)
            {
                return Json(new { success = true, message = $"User {vm.UserId} has been successfully whitelisted." });
            }
            else
            {
                return Json(new { success = false, message = "Failed to whitelist user. Please try again." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> WhitelistedUsers()
        {
            var whitelistedUsers = await _whitelist.GetWhitelistedUsersAsync();
            return PartialView("_WhitelistedUsersTable", whitelistedUsers);
        }

        [HttpPost]
        public async Task<IActionResult> RevokeAccess([FromBody] RevokeAccessViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.UserId))
            {
                return Json(new { success = false, message = "User ID is required." });
            }

            var success = await _whitelist.RevokeAccessAsync(vm.UserId);
            if (success)
            {
                return Json(new { success = true, message = $"Access revoked for user {vm.UserId}." });
            }
            else
            {
                return Json(new { success = false, message = "Failed to revoke access." });
            }
        }
    }
}
