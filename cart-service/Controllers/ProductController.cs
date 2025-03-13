using cart_service.Models;
using cart_service.Services;
using Microsoft.AspNetCore.Mvc;

namespace cart_service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductsController(ProductService productService) => 
        _productService = productService;

    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetAllProductsAsync()
    {
        return Ok(await _productService.GetAllProductsAsync());
    }

    [HttpGet("{id}")]
    [ActionName(nameof(GetProductByIdAsync))]
    public async Task<ActionResult<Product>> GetProductByIdAsync(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        return product != null ? Ok(product) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateProductAsync(Product product)
    {
        await _productService.CreateProductAsync(product);
        return CreatedAtAction(nameof(GetProductByIdAsync), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProductAsync(int id, Product product)
    {
        if (id != product.Id) return BadRequest();
        await _productService.UpdateProductAsync(product);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProductAsync(int id)
    {
        await _productService.DeleteProductAsync(id);
        return NoContent();
    }
}