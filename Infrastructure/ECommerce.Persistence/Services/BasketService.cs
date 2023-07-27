using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.DTOs.BasketItem;
using ECommerce.Application.Exceptions;
using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Persistence.Services;

public class BasketService : IBasketService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<User> _userManager;
    private readonly IRepository<Basket> _basketRepository;
    private readonly IRepository<BasketItem> _basketItemRepository;

    public BasketService(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, IRepository<Basket> basketRepository, IRepository<BasketItem> basketItemRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _basketRepository = basketRepository;
        _basketItemRepository = basketItemRepository;
    }

    private async Task<Basket> GetBasket()
    {
        var username = _httpContextAccessor.HttpContext?.User.Identity?.Name;
        
        if (string.IsNullOrEmpty(username))
            throw new NotFoundUserException();
        
        User? user = await _userManager.Users
            .Include(c => c.Baskets)
            .FirstOrDefaultAsync(c => c.UserName == username);

        var baskets = _basketRepository
            .GetWhere(x => x.UserId == user!.Id);

        Basket? targetBasket = null;
            
        if (await baskets.AnyAsync(x => x.Order == null))
            targetBasket = await baskets.FirstOrDefaultAsync(x => x.Order == null);
        else
        {
            targetBasket = new();
            user!.Baskets.Add(targetBasket);
        }
        await _basketRepository.SaveAsync();
        return targetBasket;
    }

    public async Task AddItemToBasketAsync(AddBasketItemDto item)
    {
        Basket basket = await GetBasket();

        BasketItem basketItem = await _basketItemRepository
            .GetSingleAsync(bi => bi.BasketId == basket.Id && bi.ProductId == item.ProductId);

        if (basketItem != null)
            basketItem.Quantity++;
        else
        {
            await _basketItemRepository.AddAsync(new()
            {
                BasketId = basket.Id,
                ProductId = basketItem.ProductId,
                Quantity = basketItem.Quantity
            });
            await _basketItemRepository.SaveAsync();
        }
    }

    public async Task<List<BasketItem>> GetBasketItemsAsync()
    {
        Basket basket = await GetBasket();
        Basket? result = await _basketRepository.Table
            .Include(b => b.BasketItems)
            .ThenInclude(b => b.Product)
            .FirstOrDefaultAsync(b => b.Id == basket.Id);
        return result.BasketItems.ToList();
    }

    public async Task RemoveBasketItemAsync(string basketItemId)
        => await _basketItemRepository.RemoveAsync(basketItemId);
    
    public async Task UpdateQuantityAsync(UpdateBasketItemDto item)
    {
        BasketItem? basketItem = await _basketItemRepository.GetByIdAsync(item.BasketItemId);
        if (basketItem is not null)
        {
            basketItem.Quantity = item.Quantity;
            await _basketItemRepository.SaveAsync();
        }
    }
}