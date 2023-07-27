using ECommerce.Application.Abstractions.Storage;
using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Features.Commands.ProductImage.UploadProductImage
{
    public class UploadProductImageCommandHandler : IRequestHandler<UploadProductImageCommandRequest, UploadProductImageCommandResponse>
    {
        private readonly IStorageService _storageService;
        private readonly IRepository<Domain.Entities.Product> _productRepository;
        private readonly IRepository<Domain.Entities.ProductImageFile> _productImageFileRepository;
        
        public UploadProductImageCommandHandler(IStorageService storageService, IRepository<Domain.Entities.Product> productRepository, IRepository<Domain.Entities.ProductImageFile> productImageFileRepository)
        {
            _storageService = storageService;
            _productRepository = productRepository;
            _productImageFileRepository = productImageFileRepository;
        }

        public async Task<UploadProductImageCommandResponse> Handle(UploadProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            List<(string fileName, string pathOrContainerName)> result = await _storageService.UploadAsync("photo-images", request.Files);

            Domain.Entities.Product product = await _productRepository.GetByIdAsync(request.Id);

            await _productImageFileRepository.AddRangeAsync(result.Select(r => new Domain.Entities.ProductImageFile
            {
                FileName = r.fileName,
                Path = r.pathOrContainerName,
                Storage = _storageService.StorageName,
                Products = new List<Domain.Entities.Product>() { product }
            }).ToList());

            await _productImageFileRepository.SaveAsync();

            return new();
        }
    }
}
