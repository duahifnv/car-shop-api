﻿using AutoMapper;
using cart_service.Data;
using cart_service.Dto;
using cart_service.Models;
using cart_service.Repositories;
using Microsoft.EntityFrameworkCore;

public class ProductService
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;
    private readonly AppDbContext _context; // Добавляем контекст для проверки категорий

    public ProductService(IProductRepository repository, IMapper mapper, AppDbContext context)
    {
        _repository = repository;
        _mapper = mapper;
        _context = context;
    }

    public async Task<List<ProductResponse>> GetAllProductsAsync()
    {
        var products = await _repository.GetAllAsync();
        return _mapper.Map<List<ProductResponse>>(products);
    }

    public async Task<ProductResponse> GetProductByIdAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        return _mapper.Map<ProductResponse>(product);
    }

    public async Task<ProductResponse> CreateProductAsync(ProductRequest productRequest)
    {
        // Проверяем, существует ли категория
        var categoryExists = await _context.Categories.AnyAsync(c => c.Id == productRequest.CategoryId);
        if (!categoryExists)
        {
            throw new Exception("Category not found");
        }

        var product = _mapper.Map<Product>(productRequest);
        await _repository.AddAsync(product);
        await _repository.SaveChangesAsync();
        return _mapper.Map<ProductResponse>(product);
    }

    public async Task UpdateProductAsync(int id, ProductRequest productRequest)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null) throw new Exception("Product not found");

        // Проверяем, существует ли категория
        var categoryExists = await _context.Categories.AnyAsync(c => c.Id == productRequest.CategoryId);
        if (!categoryExists)
        {
            throw new Exception("Category not found");
        }

        _mapper.Map(productRequest, product);
        await _repository.UpdateAsync(product);
        await _repository.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null) throw new Exception("Product not found");

        await _repository.DeleteAsync(id);
        await _repository.SaveChangesAsync();
    }
}