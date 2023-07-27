using ECommerce.Application.Features.Commands.Basket.AddItemToBasket;
using ECommerce.Application.Features.Commands.Basket.RemoveBasketItem;
using ECommerce.Application.Features.Commands.Basket.UpdateQuantity;
using ECommerce.Application.Features.Queries.Basket.GetBasketItems;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BasketsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BasketsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetBasketItems(GetBasketItemsQueryRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> AddItemToBasket(AddItemToBasketCommandRequest request)
    {
        await _mediator.Send(request);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateQuantiy(UpdateQuantityCommandRequest request)
    {
        await _mediator.Send(request);
        return Ok();
    }

    [HttpDelete("{BasketItemId}")]
    public async Task<IActionResult> RemoveBasketItem([FromRoute] RemoveBasketItemCommandRequest request)
    {
        await _mediator.Send(request);
        return Ok(Response);
    }
}