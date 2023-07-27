using MediatR;

namespace ECommerce.Application.Features.Commands.Basket.AddItemToBasket;

public class AddItemToBasketCommandRequest : IRequest<AddItemToBasketCommandResponse>
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}