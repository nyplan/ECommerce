namespace ECommerce.Application.Features.Queries.Basket.GetBasketItems;

public class GetBasketItemsQueryResponse
{
    public Guid BasketItemId { get; set; }
    public string Name { get; set; }
    public double TotalPrice { get; set; }
    public int Quantity { get; set; }
}