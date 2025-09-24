using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace GameStore.Api.Endpoints;

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("auth");

        // -------------------------
        // Register new user
        // -------------------------
        group.MapPost("/register", async (UserDto userDto, GameStoreContext db) =>
        {
            Console.WriteLine($"REGISTER: {userDto.Username}, {userDto.Email}");

            if (await db.Users.AnyAsync(u => u.Username == userDto.Username))
                return Results.BadRequest("Username already exists.");

            if (await db.Users.AnyAsync(u => u.Email == userDto.Email))
                return Results.BadRequest("Email already registered.");

            var user = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                Password = HashPassword(userDto.Password)
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();

            return Results.Ok(new { message = "User registered successfully" });
        });

        // -------------------------
        // Login user
        // -------------------------
        group.MapPost("/login", async (UserDto userDto, GameStoreContext db) =>
        {
            Console.WriteLine($"LOGIN attempt: {userDto.Username}");

            var user = await db.Users.FirstOrDefaultAsync(u => u.Username == userDto.Username);
            if (user == null || !VerifyPassword(userDto.Password, user.Password))
                return Results.Unauthorized();

            // Generate a dummy token (replace later with real JWT)
            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            return Results.Ok(new
            {
                token,
                message = "Login successful",
                username = user.Username,
                email = user.Email
            });
        });

        return group;
    }

    // -------------------------
    // Helpers
    // -------------------------
    private static string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        return Convert.ToBase64String(sha.ComputeHash(bytes));
    }

    private static bool VerifyPassword(string password, string hash) =>
        HashPassword(password) == hash;
}
