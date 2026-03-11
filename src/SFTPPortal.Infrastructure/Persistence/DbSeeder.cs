namespace SFTPPortal.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using SFTPPortal.Domain.Entities;
using SFTPPortal.Domain.Interfaces;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context, IAuthService authService)
    {
        // Make sure database is created
        await context.Database.MigrateAsync();

        // Only seed if no users exist
        if (await context.Users.AnyAsync())
            return;

        var users = new List<User>
        {
            new User
            {
                Username = "rami",
                PasswordHash = authService.HashPassword("rami123"),
                Entity = "ALMENA",
                Role = "Admin",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }//,
            // new User
            // {
            //     Username = "john.doe",
            //     PasswordHash = authService.HashPassword("User@123"),
            //     Entity = "Egypt",
            //     Role = "User",
            //     IsActive = true,
            //     CreatedAt = DateTime.UtcNow
            // }
        };

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();

        Console.WriteLine("✅ Database seeded successfully.");
    }
}