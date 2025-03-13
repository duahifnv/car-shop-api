using cart_service.Dto;
using cart_service.Models;
using Microsoft.AspNetCore.Mvc;

namespace cart_service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly CartService _service;

    public CartController(CartService service) => _service = service;

    [HttpGet("{userEmail}")]
    public async Task<ActionResult<CartDto>> GetCart(string userEmail)
    {
        var cart = await _service.GetCartAsync(userEmail);
        return cart != null ? Ok(cart) : NotFound();
    }

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
            return NotFound();
        }
    }

    [HttpDelete("{userEmail}/items/{productId}")]
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
    }

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
    }
}