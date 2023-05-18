using ECommerce.Application.Repositories;
using ECommerce.Persistence.Contexts;
using N = ECommerce.Domain.Entities;

namespace ECommerce.Persistence.Repositories
{
    public class ProductImageFileReadRepository : ReadRepository<N.ProductImageFile>, IProductImageFileReadRepository
    {
        public ProductImageFileReadRepository(ECommerceDbContext context) : base(context)
        {
        }
    }
}
