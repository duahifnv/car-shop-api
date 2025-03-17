using cart_service.Dto;
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
    
    [Authorize(Policy = "Authorized")]
    [HttpGet("my")]
    public async Task<ActionResult<CartResponse>> GetCurrentUserCart()
    {
        try
        {
            var username = GetCurrentUsername();
            var cart = await _service.GetCartAsync(username);
            return Ok(cart);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [Authorize(Policy = "AdminOnly")]
    [HttpGet("{username}")]
    public async Task<ActionResult<CartResponse>> GetCart(string username)
    {
        var cart = await _service.GetCartAsync(username);

        if (cart == null)
        {
            return NotFound();
        }

        return Ok(cart);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("{username}/items")]
    public async Task<IActionResult> AddItem(string username, [FromBody] CartItemDto cartItemDto)
    {
        try
        {
            await _service.AddToCartAsync(username, cartItemDto);
            return CreatedAtAction(nameof(GetCart), new { username = username }, cartItemDto);
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

    [HttpPost("my/items")]
    public async Task<IActionResult> AddItemToMyCart([FromBody] CartItemDto cartItemDto)
    {
        try
        {
            var username = GetCurrentUsername();
            await _service.AddToCartAsync(username, cartItemDto);
            return CreatedAtAction(nameof(GetCurrentUserCart), cartItemDto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{username}/items/{productId:int}")]
    public async Task<IActionResult> RemoveItem(string username, int productId)
    {
        try
        {
            await _service.RemoveFromCartAsync(username, productId);
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

    // Удалить товар из корзины текущего пользователя
    [HttpDelete("my/items/{productId}")]
    public async Task<IActionResult> RemoveItemFromMyCart(int productId)
    {
        try
        {
            var username = GetCurrentUsername();
            await _service.RemoveFromCartAsync(username, productId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Очистить корзину текущего пользователя
    [HttpDelete("my")]
    public async Task<IActionResult> ClearMyCart()
    {
        try
        {
            var username = GetCurrentUsername();
            await _service.ClearCartAsync(username);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{username}")]
    public async Task<IActionResult> ClearCart(string username)
    {
        try
        {
            await _service.ClearCartAsync(username);
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
    private string GetCurrentUsername()
    {
        var usernameClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username");
        if (usernameClaim == null)
        {
            throw new UnauthorizedAccessException("Username not found in token.");
        }
        return usernameClaim.Value;
    }
}