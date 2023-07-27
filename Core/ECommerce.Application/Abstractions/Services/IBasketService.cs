using ECommerce.Application.DTOs.BasketItem;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Abstractions.Services;

public interface IBasketService
{
    Task<List<BasketItem>> GetBasketItemsAsync();
    Task AddItemToBasketAsync(AddBasketItemDto item);
    Task UpdateQuantityAsync(UpdateBasketItemDto item);
    Task RemoveBasketItemAsync(string basketItemId);
}