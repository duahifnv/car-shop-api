﻿using System.ComponentModel.DataAnnotations;

namespace auth_service.Dtos;

public class RegisterRequest
{
    [Required]
    [StringLength(50)]
    public string Username { get; set; }

    [Required]
    [StringLength(100)]
    public string Password { get; set; }

    [Required]
    [StringLength(100)]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(20)]
    public string Role { get; set; } // Роль: "Admin" или "User"
}