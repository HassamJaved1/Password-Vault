namespace Password_Vault.Models
{
    public class LoginViewModel
    {
        public string UsernameOrUserId { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class RegisterViewModel
    {
        public string UserId { get; set; } = "";  // ACE-###
        public string Password { get; set; } = "";
        public string ConfirmPassword { get; set; } = "";
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
