public class CartResponse
{
    public string Username { get; set; }
    public List<CartItemResponse> Items { get; set; } = new();
}