using MediatR;

namespace ECommerce.Application.Features.Commands.ProductImage.DeleteProductImage
{
    public class DeleteProductImageCommandRequest : IRequest<DeleteProductImageCommandResponse>
    {
        public Guid Id { get; set; }
        public Guid ImageId { get; set; }
    }
}
