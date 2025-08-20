using Dapper;
using Password_Vault.Models;
using System.Data;

namespace Password_Vault.Data
{
    public interface IAdminRepository
    {
        Task<Admin?> GetByUsernameAsync(string username);
    }

    public class AdminRepository : IAdminRepository
    {
        private readonly IDbConnection _db;
        public AdminRepository(IDbConnection db) => _db = db;

        public Task<Admin?> GetByUsernameAsync(string username) =>
            _db.QueryFirstOrDefaultAsync<Admin>(
                "SELECT * FROM admins WHERE username = @u", new { u = username });
    }
}
