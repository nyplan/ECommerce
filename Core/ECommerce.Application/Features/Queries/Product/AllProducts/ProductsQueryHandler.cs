﻿using ECommerce.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Queries.Product.GetAllProducts
{
    public class ProductsQueryHandler : IRequestHandler<ProductsQueryRequest, ProductsQueryResponse>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly ILogger<ProductsQueryHandler> _logger;
        public ProductsQueryHandler(IProductReadRepository productReadRepository, ILogger<ProductsQueryHandler> logger)
        {
            _productReadRepository = productReadRepository;
            _logger = logger;
        }
        public async Task<ProductsQueryResponse> Handle(ProductsQueryRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get all products");

            var totalProductCount = _productReadRepository.GetAll(false).Count();

            var products = _productReadRepository.GetAll(false).Skip(request.Page * request.Size).Take(request.Size)
                .Include(p => p.Images)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Stock,
                    p.Price,
                    p.CreatedDate,
                    p.UpdatedDate,
                    p.Images
                }).ToList();

            return new()
            {
                Products = products,
                TotalProductCount = totalProductCount
            };
        }
    }
}
