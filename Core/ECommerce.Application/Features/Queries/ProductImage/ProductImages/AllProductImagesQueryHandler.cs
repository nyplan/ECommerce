using ECommerce.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Application.Features.Queries.ProductImage.ProductImages
{
    public class AllProductImagesQueryHandler : IRequestHandler<AllProductImagesQueryRequest, List<AllProductImagesQueryResponse>>
    {
        private readonly IRepository<Domain.Entities.Product> _productRepository;
        private readonly IConfiguration _configuration;
        public AllProductImagesQueryHandler(IRepository<Domain.Entities.Product> productRepository, IConfiguration configuration)
        {
            _productRepository = productRepository;
            _configuration = configuration;
        }

        public async Task<List<AllProductImagesQueryResponse>> Handle(AllProductImagesQueryRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Product? product = await _productRepository.Table.Include(p => p.Images)
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

