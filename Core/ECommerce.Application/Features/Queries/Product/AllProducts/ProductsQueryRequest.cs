using MediatR;

namespace ECommerce.Application.Features.Queries.Product.GetAllProducts
{
    public class ProductsQueryRequest : IRequest<ProductsQueryResponse>
    {
        //public Pagination Pagination { get; set; }
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 5;
    }
}
