using ECommerce.Application.Repositories;
using ECommerce.Persistence.Contexts;
using N = ECommerce.Domain.Entities;
namespace ECommerce.Persistence.Repositories
{
    public class FileWriteRepository : WriteRepository<N.File>, IFileWriteRepository
    {
        public FileWriteRepository(ECommerceDbContext context) : base(context)
        {
        }
    }
}
