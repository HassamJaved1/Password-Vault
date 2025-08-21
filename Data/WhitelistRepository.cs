using Dapper;
using Password_Vault.Models;
using System.Data;

namespace Password_Vault.Data
{
    public interface IWhitelistRepository
    {
        Task<bool> IsWhitelistedAsync(string userId);
        Task<bool> AddToWhitelistAsync(long adminId, string userId);
        Task<IEnumerable<WhitelistedUser>> GetWhitelistedUsersAsync();
        Task<bool> RevokeAccessAsync(string userId);
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

        public async Task<bool> AddToWhitelistAsync(long adminId, string userId)
        {
            try
            {
                var result = await _db.ExecuteAsync(
                    @"INSERT INTO whitelisted_users (admin_id, user_id, created_at, updated_at, access_revoked) 
                      VALUES (@adminId, @userId, NOW(), NOW(), 0)",
                    new { adminId, userId = userId.ToUpperInvariant() });
                return result > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<WhitelistedUser>> GetWhitelistedUsersAsync()
        {
            return await _db.QueryAsync<WhitelistedUser>(
                @"SELECT * FROM whitelisted_users ORDER BY created_at DESC");
        }

        public async Task<bool> RevokeAccessAsync(string userId)
        {
            var result = await _db.ExecuteAsync(
                @"UPDATE whitelisted_users SET access_revoked = 1, updated_at = NOW() 
                  WHERE user_id = @userId",
                new { userId });
            return result > 0;
        }
    }
}
