using cart_service.Dto;
using cart_service.Models;
using cart_service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cart_service.Controllers;

[ApiController]
[Route("api/cart/categories")]
public class CategoriesController : ControllerBase
{
    private readonly CategoryService _service;

    public CategoriesController(CategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<Category>>> GetAll()
    {
        var categories = await _service.GetAllCategoriesAsync();
        return Ok(categories);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Category>> GetById(int id)
    {
        var category = await _service.GetCategoryByIdAsync(id);
        return category != null ? Ok(category) : NotFound();
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public async Task<IActionResult> Create(CategoryRequest categoryRequest)
    {
        try
        {
            var category = await _service.CreateCategoryAsync(categoryRequest);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CategoryRequest categoryRequest)
    {
        try
        {
            await _service.UpdateCategoryAsync(id, categoryRequest);
            return NoContent();
        }
        catch (Exception ex) when (ex.Message == "Category not found")
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.DeleteCategoryAsync(id);
            return NoContent();
        }
        catch (Exception ex) when (ex.Message == "Category not found")
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}