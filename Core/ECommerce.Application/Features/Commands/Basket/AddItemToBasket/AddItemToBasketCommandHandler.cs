using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.DTOs.BasketItem;
using MediatR;

namespace ECommerce.Application.Features.Commands.Basket.AddItemToBasket;

public class AddItemToBasketCommandHandler : IRequestHandler<AddItemToBasketCommandRequest, AddItemToBasketCommandResponse>
{
    private readonly IBasketService _basketService;

    public AddItemToBasketCommandHandler(IBasketService basketService)
    {
        _basketService = basketService;
    }

    public async Task<AddItemToBasketCommandResponse> Handle(AddItemToBasketCommandRequest request, CancellationToken cancellationToken)
    {
        await _basketService.AddItemToBasketAsync(new AddBasketItemDto()
        {
            ProductId = request.ProductId,
            Quantity = request.Quantity
        });
        return new();
    }
}