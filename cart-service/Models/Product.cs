﻿using System.ComponentModel.DataAnnotations;

namespace cart_service.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public int? CategoryId { get; set; }
    public Category Category { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}