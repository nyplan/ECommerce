using ECommerce.Application.Repositories;
using ECommerce.Persistence.Contexts;
using n = ECommerce.Domain.Entities;

namespace ECommerce.Persistence.Repositories
{
    public class InvoiceFileReadRepository : ReadRepository<n.InvoiceFile>, IInvoiceFileReadRepository
    {
        public InvoiceFileReadRepository(ECommerceDbContext context) : base(context)
        {
        }
    }
}
