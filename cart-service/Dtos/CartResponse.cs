namespace cart_service.Dto;

public class CartResponse
{
    public string Username { get; set; }
    public List<CartItemDto> Items { get; set; } = new();
}