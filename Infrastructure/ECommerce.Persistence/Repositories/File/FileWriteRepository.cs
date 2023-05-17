using ECommerce.Application.Repositories;
using ECommerce.Persistence.Contexts;
using n = ECommerce.Domain.Entities;
namespace ECommerce.Persistence.Repositories
{
    public class FileWriteRepository : WriteRepository<n.File>, IFileWriteRepository
    {
        public FileWriteRepository(ECommerceDbContext context) : base(context)
        {
        }
    }
}
