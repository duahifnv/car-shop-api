using cart_service.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/cart/products")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _service;

    public ProductsController(ProductService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetAll() => Ok(await _service.GetAllProductsAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductResponse>> GetById(int id)
    {
        var product = await _service.GetProductByIdAsync(id);
        return product != null ? Ok(product) : NotFound();
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public async Task<IActionResult> Create(ProductRequest productRequest)
    {
        try
        {
            var productDto = await _service.CreateProductAsync(productRequest);
            return CreatedAtAction(nameof(GetById), new { id = productDto.Id }, productDto);
        }
        catch (Exception ex) when (ex.Message == "Category not found")
        {
            return BadRequest("Category not found");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while creating the product.");
        }
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ProductRequest productRequest)
    {
        try
        {
            await _service.UpdateProductAsync(id, productRequest);
            return NoContent();
        }
        catch (Exception ex) when (ex.Message == "Product not found")
        {
            return NotFound();
        }
        catch (Exception ex) when (ex.Message == "Category not found")
        {
            return BadRequest("Category not found");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while updating the product.");
        }
    }
    
    [Authorize(Policy = "AdminOnly")]
    [HttpPatch("{id:int}/stock")]
    public async Task<IActionResult> UpdateStockQuantity(int id, [FromQuery] int quantity)
    {
        try
        {
            await _service.UpdateStockQuantityAsync(id, quantity);
            return NoContent();
        }
        catch (Exception ex) when (ex.Message == "Product not found")
        {
            return NotFound();
        }
        catch (Exception ex) when (ex.Message == "Quantity cannot be negative")
        {
            return BadRequest("Quantity cannot be negative");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while updating the stock quantity.");
        }
    }
    
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.DeleteProductAsync(id);
            return NoContent();
        }
        catch (Exception ex) when (ex.Message == "Product not found")
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while deleting the product.");
        }
    }
}