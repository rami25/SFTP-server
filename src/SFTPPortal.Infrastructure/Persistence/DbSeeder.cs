namespace SFTPPortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using SFTPPortal.Domain.Entities;
using SFTPPortal.Domain.Interfaces;

public static class DbSeeder {
    public static async Task SeedAsync(AppDbContext context, IAuthService authService) {
        await context.Database.MigrateAsync(); // make sure database is created

        if (await context.Users.AnyAsync()) // only seed if no users exist
            return;

        var users = new List<User> {
            new User {
                Username = "rami",
                PasswordHash = authService.HashPassword("rami123"),
                Entity = "Tunisia",
                Role = "Admin",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();

        Console.WriteLine("Database seeded successfully.");
    }
}