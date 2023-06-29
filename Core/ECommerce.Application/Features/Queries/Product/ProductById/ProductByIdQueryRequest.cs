using MediatR;

namespace ECommerce.Application.Features.Queries.Product.ById
{
    public class ProductByIdQueryRequest : IRequest<ProductByIdQueryResponse>
    {
        public string Id { get; set; }
    }
}
