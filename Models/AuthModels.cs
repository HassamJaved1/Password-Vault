using System.ComponentModel.DataAnnotations;

namespace Password_Vault.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username or User ID is required.")]
        public string UsernameOrUserId { get; set; } = "";
        
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = "";
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "User ID is required.")]
        [RegularExpression(@"^ACE-\d{4}$", ErrorMessage = "User ID must be in format ACE-XXXX (e.g., ACE-1234).")]
        public string UserId { get; set; } = "";  // ACE-###
        
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; } = "";
        
        [Required(ErrorMessage = "Password confirmation is required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = "";
    }

    public class WhitelistUserViewModel
    {
        [Required(ErrorMessage = "User ID is required.")]
        [RegularExpression(@"^ACE-\d{4}$", ErrorMessage = "User ID must be in format ACE-XXXX (e.g., ACE-1234).")]
        public string UserId { get; set; } = "";  // ACE-###
        
        [Required(ErrorMessage = "You must confirm that you want to whitelist this user.")]
        public bool ConfirmWhitelist { get; set; }
    }

    public class RevokeAccessViewModel
    {
        [Required(ErrorMessage = "User ID is required.")]
        public string UserId { get; set; } = "";
    }

    public class Admin
    {
        public long Id { get; set; }
        public string Username { get; set; } = "";
        public string Password_Hash { get; set; } = "";
    }

    public class AppUser
    {
        public long Id { get; set; }
        public string User_Id { get; set; } = "";
        public string Password_Hash { get; set; } = "";
    }

    public class WhitelistedUser
    {
        public long Id { get; set; }
        public long Admin_Id { get; set; }
        public string User_Id { get; set; } = "";
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }
        public bool Access_Revoked { get; set; }
    }

}
