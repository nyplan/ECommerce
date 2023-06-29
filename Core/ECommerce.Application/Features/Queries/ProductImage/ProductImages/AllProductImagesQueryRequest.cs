using MediatR;

namespace ECommerce.Application.Features.Queries.ProductImage.ProductImages
{
    public class AllProductImagesQueryRequest : IRequest<List<AllProductImagesQueryResponse>>
    {
        public Guid Id { get; set; }
    }
}
