using ECommerce.Application.Repositories;
using ECommerce.Persistence.Contexts;
using N = ECommerce.Domain.Entities;

namespace ECommerce.Persistence.Repositories
{
    public class InvoiceFileReadRepository : ReadRepository<N.InvoiceFile>, IInvoiceFileReadRepository
    {
        public InvoiceFileReadRepository(ECommerceDbContext context) : base(context)
        {
        }
    }
}
