using Dapper;
using Password_Vault.Models;
using System.Data;

namespace Password_Vault.Data
{
    public interface IUserRepository
    {
        Task<AppUser?> GetByUserIdAsync(string userId);
        Task<long> CreateAsync(string userId, string passwordHash);
    }

    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _db;
        public UserRepository(IDbConnection db) => _db = db;

        public Task<AppUser?> GetByUserIdAsync(string userId) =>
            _db.QueryFirstOrDefaultAsync<AppUser>(
                "SELECT * FROM users WHERE user_id = @id", new { id = userId });

        public async Task<long> CreateAsync(string userId, string passwordHash)
        {
            var sql = @"INSERT INTO users (user_id, password_hash)
                    VALUES (@u,@p);
                    SELECT LAST_INSERT_ID();";
            return await _db.ExecuteScalarAsync<long>(sql, new { u = userId, p = passwordHash });
        }
    }
}
