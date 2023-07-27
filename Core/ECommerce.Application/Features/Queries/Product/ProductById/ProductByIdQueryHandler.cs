using ECommerce.Application.Repositories;
using MediatR;

namespace ECommerce.Application.Features.Queries.Product.ById
{
    public class ProductByIdQueryHandler : IRequestHandler<ProductByIdQueryRequest, ProductByIdQueryResponse>
    {
        private readonly IRepository<Domain.Entities.Product> _productRepository;
        public ProductByIdQueryHandler(IRepository<Domain.Entities.Product> productRepository)
        {
            _productRepository = productRepository;
        }

        async Task<ProductByIdQueryResponse> IRequestHandler<ProductByIdQueryRequest, ProductByIdQueryResponse>.Handle(ProductByIdQueryRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Product product = await _productRepository.GetByIdAsync(request.Id, false);
            return new() 
            {
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock
            };
        }
    }
}
