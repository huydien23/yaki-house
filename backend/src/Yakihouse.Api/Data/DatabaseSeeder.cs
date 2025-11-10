using Microsoft.EntityFrameworkCore;
using Yakihouse.Application.Common.Interfaces;
using Yakihouse.Domain.Entities;
using Yakihouse.Infrastructure.Persistence;

namespace Yakihouse.Api.Data;

public static class DatabaseSeeder
{
    public static async Task SeedDataAsync(YakihouseDbContext context, IPasswordHasher passwordHasher)
    {
        // Check if Users table exists and has data
        if (await context.Users.AnyAsync())
        {
            return; // Database already seeded
        }

        // Seed Users
        // Password: Admin@123
        var passwordHash = passwordHasher.HashPassword("Admin@123");

        var users = new List<User>
        {
            new User
            {
                Id = Guid.NewGuid(),
                Username = "admin",
                PasswordHash = passwordHash,
                FullName = "Quản trị viên",
                Email = "admin@yakihouse.com",
                Role = "Admin",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.NewGuid(),
                Username = "manager",
                PasswordHash = passwordHash,
                FullName = "Quản lý",
                Email = "manager@yakihouse.com",
                Role = "Manager",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.NewGuid(),
                Username = "staff1",
                PasswordHash = passwordHash,
                FullName = "Nhân viên 1",
                Email = "staff1@yakihouse.com",
                Role = "Staff",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.NewGuid(),
                Username = "kitchen1",
                PasswordHash = passwordHash,
                FullName = "Bếp trưởng",
                Email = "kitchen1@yakihouse.com",
                Role = "Kitchen",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();
    }
}
