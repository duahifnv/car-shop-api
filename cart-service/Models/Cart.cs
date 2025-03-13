﻿using System.ComponentModel.DataAnnotations;

namespace cart_service.Models;

public class Cart
{
    public string UserEmail { get; set; }
    public List<CartItem> Items { get; set; } = new();
}