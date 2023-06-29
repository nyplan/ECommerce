using ECommerce.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Application.Features.Queries.ProductImage.ProductImages
{
    public class AllProductImagesQueryHandler : IRequestHandler<AllProductImagesQueryRequest, List<AllProductImagesQueryResponse>>
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IConfiguration _configuration;
        public AllProductImagesQueryHandler(IProductReadRepository productReadRepository, IConfiguration configuration)
        {
            _productReadRepository = productReadRepository;
            _configuration = configuration;
        }

        public async Task<List<AllProductImagesQueryResponse>> Handle(AllProductImagesQueryRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Product? product = await _productReadRepository.Table.Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == request.Id);
            return product.Images.Select(p => new AllProductImagesQueryResponse()
            {
                Id = p.Id,
                FileName = p.FileName,
                Path = $"{_configuration["BaseStorageUrl"]}/{p.Path}"
            }).ToList();
        }
    }
}

