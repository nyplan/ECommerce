namespace ECommerce.Application.DTOs.BasketItem;

public class AddBasketItemDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}