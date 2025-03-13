namespace cart_service.Dto;

public class CartDto
{
    public string UserEmail { get; set; }
    public List<CartItemDto> Items { get; set; } = new();
}