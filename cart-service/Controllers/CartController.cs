﻿using cart_service.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/cart")]
public class CartController : ControllerBase
{
    private readonly CartService _service;

    public CartController(CartService service)
    {
        _service = service;
    }
    
    [Authorize(Policy = "AdminOnly")]
    [HttpGet("{userEmail}")]
    public async Task<ActionResult<CartResponse>> GetCart(string userEmail)
    {
        var cart = await _service.GetCartAsync(userEmail);

        if (cart == null)
        {
            return NotFound();
        }

        return Ok(cart);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("{userEmail}/items")]
    public async Task<IActionResult> AddItem(string userEmail, [FromBody] CartItemDto cartItemDto)
    {
        try
        {
            await _service.AddToCartAsync(userEmail, cartItemDto);
            return CreatedAtAction(nameof(GetCart), new { userEmail }, cartItemDto);
        }
        catch (Exception ex) when (ex.Message == "Product not found")
        {
            return BadRequest("Product not found");
        }
        catch (Exception ex) when (ex.Message == "Not enough stock available")
        {
            return BadRequest("Not enough stock available");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while adding the item to the cart.");
        }
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{userEmail}/items/{productId:int}")]
    public async Task<IActionResult> RemoveItem(string userEmail, int productId)
    {
        try
        {
            await _service.RemoveFromCartAsync(userEmail, productId);
            return NoContent();
        }
        catch (Exception ex) when (ex.Message == "Item not found")
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while removing the item from the cart.");
        }
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{userEmail}")]
    public async Task<IActionResult> ClearCart(string userEmail)
    {
        try
        {
            await _service.ClearCartAsync(userEmail);
            return NoContent();
        }
        catch (Exception ex) when (ex.Message == "Cart not found")
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while clearing the cart.");
        }
    }
}