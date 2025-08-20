using Dapper;
using System.Data;

namespace Password_Vault.Data
{
    public interface IWhitelistRepository
    {
        Task<bool> IsWhitelistedAsync(string userId);
    }

    public class WhitelistRepository : IWhitelistRepository
    {
        private readonly IDbConnection _db;
        public WhitelistRepository(IDbConnection db) => _db = db;

        public async Task<bool> IsWhitelistedAsync(string userId)
        {
            // case-insensitive match on stored text
            var cnt = await _db.ExecuteScalarAsync<int>(
                @"SELECT COUNT(*) FROM whitelisted_users 
              WHERE LOWER(user_id) = LOWER(@u) AND access_revoked = 0",
                new { u = userId });
            return cnt > 0;
        }
    }
}
