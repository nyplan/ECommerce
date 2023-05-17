using ECommerce.Application.Repositories;
using ECommerce.Persistence.Contexts;
using n = ECommerce.Domain.Entities;

namespace ECommerce.Persistence.Repositories
{
    public class FileReadRepository : ReadRepository<n.File>, IFileReadRepository
    {
        public FileReadRepository(ECommerceDbContext context) : base(context)
        {
        }
    }
}
