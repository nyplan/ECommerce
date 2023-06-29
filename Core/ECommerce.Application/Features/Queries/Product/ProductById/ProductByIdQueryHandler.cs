using ECommerce.Application.Repositories;
using MediatR;

namespace ECommerce.Application.Features.Queries.Product.ById
{
    public class ProductByIdQueryHandler : IRequestHandler<ProductByIdQueryRequest, ProductByIdQueryResponse>
    {
        private readonly IProductReadRepository _productReadRepository;
        public ProductByIdQueryHandler(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;
        }

        async Task<ProductByIdQueryResponse> IRequestHandler<ProductByIdQueryRequest, ProductByIdQueryResponse>.Handle(ProductByIdQueryRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Product product = await _productReadRepository.GetByIdAsync(request.Id, false);
            return new() 
            {
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock
            };
        }
    }
}
