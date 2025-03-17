using auth_service.Models;
using Microsoft.EntityFrameworkCore;

namespace auth_service.Data;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
    
    public async Task Initialize()
    {
        // Проверяем, есть ли уже категории в базе данных
        if (!Users.Any())
        {
            // Добавляем начальные категории
            var users = new List<User>
            {
                new() { 
                    Username = "admin", 
                    Password = BCrypt.Net.BCrypt.HashPassword("admin"),
                    Email = "admin@mail.ru",
                    Role = "Admin" },
                new()
                {
                    Username = "user",
                    Password = BCrypt.Net.BCrypt.HashPassword("user"),
                    Email = "user@mail.ru",
                    Role = "User"
                }
            };

            await Users.AddRangeAsync(users);
            await SaveChangesAsync();
        }
    }
}