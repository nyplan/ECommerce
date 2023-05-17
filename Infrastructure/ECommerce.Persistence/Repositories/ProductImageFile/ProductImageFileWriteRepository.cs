using ECommerce.Application.Repositories;
using ECommerce.Persistence.Contexts;
using n = ECommerce.Domain.Entities;

namespace ECommerce.Persistence.Repositories
{
    public class ProductImageFileWriteRepository : WriteRepository<n.ProductImageFile>, IProductImageFileWriteRepository
    {
        public ProductImageFileWriteRepository(ECommerceDbContext context) : base(context)
        {
        }
       
    }
}
