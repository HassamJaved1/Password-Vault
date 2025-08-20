using System.Data;
using Dapper;
using MySql.Data.MySqlClient;
using Password_Vault.Data;
using Password_Vault.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IDbConnection>(sp =>
{
    var cs = builder.Configuration.GetConnectionString("Default");
    return new MySqlConnection(cs);
});

builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IWhitelistRepository, WhitelistRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", opts =>
    {
        opts.LoginPath = "/Account/Login";
        opts.AccessDeniedPath = "/Account/Login";
    });

var app = builder.Build();

await EnsureAdminSeed(app.Services, app.Configuration);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();

static async Task EnsureAdminSeed(IServiceProvider services, IConfiguration cfg)
{
    using var scope = services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<IDbConnection>();
    var username = cfg["AdminSeed:Username"]!;
    var password = cfg["AdminSeed:Password"]!;

    var exists = await db.ExecuteScalarAsync<int>(
        "SELECT COUNT(*) FROM admins WHERE username = @u", new { u = username });

    if (exists == 0)
    {
        var hash = BCrypt.Net.BCrypt.HashPassword(password);
        await db.ExecuteAsync(
            "INSERT INTO admins (username, password_hash) VALUES (@u, @p)",
            new { u = username, p = hash });
    }
}
