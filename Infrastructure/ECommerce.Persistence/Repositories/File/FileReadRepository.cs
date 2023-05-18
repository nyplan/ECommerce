using ECommerce.Application.Repositories;
using ECommerce.Persistence.Contexts;
using N = ECommerce.Domain.Entities;

namespace ECommerce.Persistence.Repositories
{
    public class FileReadRepository : ReadRepository<N.File>, IFileReadRepository
    {
        public FileReadRepository(ECommerceDbContext context) : base(context)
        {
        }
    }
}
