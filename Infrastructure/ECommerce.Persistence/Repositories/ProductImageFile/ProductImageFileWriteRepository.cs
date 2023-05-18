using ECommerce.Application.Repositories;
using ECommerce.Persistence.Contexts;
using N = ECommerce.Domain.Entities;

namespace ECommerce.Persistence.Repositories
{
    public class ProductImageFileWriteRepository : WriteRepository<N.ProductImageFile>, IProductImageFileWriteRepository
    {
        public ProductImageFileWriteRepository(ECommerceDbContext context) : base(context)
        {
        }
       
    }
}
