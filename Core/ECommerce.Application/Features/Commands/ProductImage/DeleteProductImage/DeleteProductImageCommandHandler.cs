using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Features.Commands.ProductImage.DeleteProductImage
{
    public class DeleteProductImageCommandHandler : IRequestHandler<DeleteProductImageCommandRequest, DeleteProductImageCommandResponse>
    {
        private readonly IRepository<Domain.Entities.Product> _productRepository;
        public DeleteProductImageCommandHandler(IRepository<Domain.Entities.Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<DeleteProductImageCommandResponse> Handle(DeleteProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Product? product = await _productRepository.Table.Include(p => p.Images)
               .FirstOrDefaultAsync(p => p.Id == request.Id);
            Domain.Entities.ProductImageFile? imageFile = product?.Images.FirstOrDefault(p => p.Id == request.ImageId);
            if (imageFile is not null)
                product?.Images.Remove(imageFile);
            await _productRepository.SaveAsync();
            return new();
        }
    }
}
