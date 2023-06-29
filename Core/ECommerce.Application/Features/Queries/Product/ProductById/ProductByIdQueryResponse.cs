using ECommerce.Domain.Entities;

namespace ECommerce.Application.Features.Queries.Product.ById
{
    public class ProductByIdQueryResponse
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
    }
}
