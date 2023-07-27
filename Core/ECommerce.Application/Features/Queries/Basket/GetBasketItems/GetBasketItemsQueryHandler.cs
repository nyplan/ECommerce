using ECommerce.Application.Abstractions.Services;
using MediatR;

namespace ECommerce.Application.Features.Queries.Basket.GetBasketItems;

public class GetBasketItemsQueryHandler : IRequestHandler<GetBasketItemsQueryRequest, List<GetBasketItemsQueryResponse>>
{
    private readonly IBasketService _basketService;

    public GetBasketItemsQueryHandler(IBasketService basketService)
    {
        _basketService = basketService;
    }

    public async Task<List<GetBasketItemsQueryResponse>> Handle(GetBasketItemsQueryRequest request, CancellationToken cancellationToken)
    {
        var basketItems = await _basketService.GetBasketItemsAsync();
        return basketItems.Select(ba => new GetBasketItemsQueryResponse()
        {
            BasketItemId = ba.Id,
            Name = ba.Product.Name,
            TotalPrice = ba.Product.Price * ba.Quantity,
            Quantity = ba.Quantity
        }).ToList();
    }
}