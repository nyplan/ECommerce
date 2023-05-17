using ECommerce.Application.Repositories;
using ECommerce.Persistence.Contexts;
using n = ECommerce.Domain.Entities;

namespace ECommerce.Persistence.Repositories
{
    public class ProductImageFileReadRepository : ReadRepository<n.ProductImageFile>, IProductImageFileReadRepository
    {
        public ProductImageFileReadRepository(ECommerceDbContext context) : base(context)
        {
        }
    }
}
