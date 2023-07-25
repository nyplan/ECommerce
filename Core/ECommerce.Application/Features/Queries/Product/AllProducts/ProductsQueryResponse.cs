namespace ECommerce.Application.Features.Queries.Product.GetAllProducts
{
    public class ProductsQueryResponse
    {
        public int TotalProductCount { get; set; }
        public object Products { get; set; }
    }
}
