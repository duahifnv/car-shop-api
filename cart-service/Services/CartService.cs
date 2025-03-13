using AutoMapper;
using cart_service.Dto;
using cart_service.Models;
using cart_service.Repositories;

public class CartService
{
    private readonly ICartRepository _repository;
    private readonly IMapper _mapper;

    public CartService(ICartRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CartDto> GetCartAsync(string userEmail)
    {
        var cart = await _repository.GetCartAsync(userEmail);
        if (cart == null) return null;
        return _mapper.Map<CartDto>(cart);
    }

    public async Task AddToCartAsync(string userEmail, CartItemDto cartItemDto)
    {
        var product = await _repository.GetProductByIdAsync(cartItemDto.ProductId);
        if (product == null) throw new Exception("Product not found");

        var cartItem = _mapper.Map<CartItem>(cartItemDto);
        await _repository.AddOrUpdateItemAsync(userEmail, cartItem);
    }

    public async Task RemoveFromCartAsync(string userEmail, int productId)
    {
        var item = await _repository.GetCartItemAsync(userEmail, productId);
        if (item == null) throw new Exception("Item not found");

        await _repository.RemoveItemAsync(userEmail, productId);
    }

    public async Task ClearCartAsync(string userEmail)
    {
        var cart = await _repository.GetCartAsync(userEmail);
        if (cart == null) throw new Exception("Cart not found");

        await _repository.ClearCartAsync(userEmail);
    }
}