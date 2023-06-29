using ECommerce.Application.Repositories;
using MediatR;

namespace ECommerce.Application.Features.Queries.Product.GetAllProducts
{
    public class ProductsQueryHandler : IRequestHandler<ProductsQueryRequest, ProductsQueryResponse>
    {
        private readonly IProductReadRepository _productReadRepository;
        public ProductsQueryHandler(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;
        }

        public async Task<ProductsQueryResponse> Handle(ProductsQueryRequest request, CancellationToken cancellationToken)
        {
            var totalCount = _productReadRepository.GetAll(false).Count();
            var products = _productReadRepository.GetAll(false).Skip(request.Page * request.Size).Take(request.Size).Select(p => new
            {
                p.Id,
                p.Name,
                p.Stock,
                p.Price,
                p.CreatedDate,
                p.UpdatedDate
            }).ToList();

            return new()
            {
                Products = products,
                TotalCount = totalCount
            };
        }
    }
}
