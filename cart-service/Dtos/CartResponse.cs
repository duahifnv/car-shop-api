namespace cart_service.Dto;

public class CartResponse
{
    public string UserEmail { get; set; }
    public List<CartItemDto> Items { get; set; } = new();
}