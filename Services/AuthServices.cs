namespace Password_Vault.Services
{
    public interface IAuthService
    {
        bool ValidateUserIdFormat(string userId);
        bool VerifyPassword(string plain, string hash);
        string HashPassword(string plain);
    }

    public class AuthService : IAuthService
    {
        private static readonly System.Text.RegularExpressions.Regex UserIdRegex =
      new(@"^ACE-\d{1,4}$", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled);

        public bool ValidateUserIdFormat(string userId) => UserIdRegex.IsMatch(userId ?? "");

        public bool VerifyPassword(string plain, string hash) =>
            BCrypt.Net.BCrypt.Verify(plain, hash);

        public string HashPassword(string plain) =>
            BCrypt.Net.BCrypt.HashPassword(plain);
    }

}
