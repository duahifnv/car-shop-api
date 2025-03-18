using AutoMapper;
using cart_service.Dto;
using cart_service.Models;
using cart_service.Repositories;

public class CartService
{
    private readonly ICartRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<CartService> _logger;
    
    public CartService(ICartRepository repository, IMapper mapper, ILogger<CartService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CartResponse> GetCartAsync(string username)
    {
        var cart = await _repository.GetCartAsync(username);
        if (cart == null) return null;
        return _mapper.Map<CartResponse>(cart);
    }

    public async Task AddToCartAsync(string username, CartItemDto cartItemDto)
    {
        var product = await _repository.GetProductByIdAsync(cartItemDto.ProductId);
        if (product == null) throw new Exception("Product not found");
        if (product.StockQuantity < cartItemDto.Quantity)
        {
            throw new Exception("Not enough stock available");
        }
        var cartItem = _mapper.Map<CartItem>(cartItemDto);
        await _repository.AddOrUpdateItemAsync(username, cartItem);
        _logger.LogInformation("Product #{product} (x{quantity}) added to cart of user: {cart}",
            cartItem.Product.Id, cartItem.Quantity, username);
    }

    public async Task RemoveFromCartAsync(string username, int productId)
    {
        var item = await _repository.GetCartItemAsync(username, productId);
        if (item == null) throw new Exception("Item not found");

        await _repository.RemoveItemAsync(username, productId);
        _logger.LogInformation("Product #{product} removed from cart of user: {cart}", productId, username);
    }

    public async Task ClearCartAsync(string username)
    {
        var cart = await _repository.GetCartAsync(username);
        if (cart == null) throw new Exception("Cart not found");

        await _repository.ClearCartAsync(username);
        _logger.LogInformation("Cart of user: {cart} cleared", username);

    }
}