﻿using cart_service.Data;
using cart_service.Models;
using cart_service.Repositories;
using Microsoft.EntityFrameworkCore;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;

    public CartRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Cart> GetCartAsync(string username)
    {
        var items = await _context.CartItems
            .Include(ci => ci.Product)
            .Where(ci => ci.Username == username)
            .ToListAsync();
        
        if (items == null) return null;
        
        return new Cart { Username = username, Items = items };
    }

    public async Task AddOrUpdateItemAsync(string username, CartItem item)
    {
        var existingItem = await _context.CartItems.FindAsync(username, item.ProductId);
        
        if (existingItem != null)
        {
            existingItem.Quantity += item.Quantity;
        }
        else
        {
            item.Username = username;
            await _context.CartItems.AddAsync(item);
        }
        
        await _context.SaveChangesAsync();
    }

    public async Task RemoveItemAsync(string username, int productId)
    {
        var item = await _context.CartItems.FindAsync(username, productId);
        if (item != null)
        {
            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ClearCartAsync(string username)
    {
        var items = await _context.CartItems
            .Where(ci => ci.Username == username)
            .ToListAsync();
            
        _context.CartItems.RemoveRange(items);
        await _context.SaveChangesAsync();
    }

    public async Task<Product> GetProductByIdAsync(int productId)
    {
        return await _context.Products.FindAsync(productId);
    }

    public async Task<CartItem> GetCartItemAsync(string username, int productId)
    {
        return await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.Username == username && ci.ProductId == productId);
    }
}