using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Features.Commands.ProductImage.DeleteProductImage
{
    public class DeleteProductImageCommandHandler : IRequestHandler<DeleteProductImageCommandRequest, DeleteProductImageCommandResponse>
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        public DeleteProductImageCommandHandler(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
        }

        public async Task<DeleteProductImageCommandResponse> Handle(DeleteProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Product? product = await _productReadRepository.Table.Include(p => p.Images)
               .FirstOrDefaultAsync(p => p.Id == request.Id);
            ProductImageFile? imageFile = product?.Images.FirstOrDefault(p => p.Id == request.ImageId);
            if (imageFile is not null)
                product?.Images.Remove(imageFile);
            await _productWriteRepository.SaveAsync();
            return new();
        }
    }
}
