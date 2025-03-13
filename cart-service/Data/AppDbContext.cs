using cart_service.Models;
using Microsoft.EntityFrameworkCore;

namespace cart_service.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CartItem>()
            .HasKey(ci => new { ci.UserEmail, ci.ProductId });
        modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId);
    }

    public async Task Initialize()
    {
        // Проверяем, есть ли уже категории в базе данных
        if (!Categories.Any())
        {
            // Добавляем начальные категории
            var categories = new List<Category>
            {
                new() { Name = "Легковые автомобили", Description = "Легковые автомобили различных марок" },
                new() { Name = "Грузовые автомобили", Description = "Грузовые автомобили для перевозки грузов" },
                new() { Name = "Мотоциклы", Description = "Мотоциклы и скутеры" },
                new() { Name = "Запчасти", Description = "Запчасти для автомобилей и мотоциклов" },
                new() { Name = "Аксессуары", Description = "Аксессуары для автомобилей" }
            };

            await Categories.AddRangeAsync(categories);
            await SaveChangesAsync();
        }
    }
}