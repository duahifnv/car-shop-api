using cart_service.Models;
using Microsoft.EntityFrameworkCore;

namespace cart_service.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Product> Products { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
}