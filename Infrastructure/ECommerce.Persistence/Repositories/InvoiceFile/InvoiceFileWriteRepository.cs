using ECommerce.Application.Repositories;
using ECommerce.Persistence.Contexts;
using N = ECommerce.Domain.Entities;


namespace ECommerce.Persistence.Repositories
{
    internal class InvoiceFileWriteRepository : WriteRepository<N.InvoiceFile>, IInvoiceFileWriteRepository
    {
        public InvoiceFileWriteRepository(ECommerceDbContext context) : base(context)
        {
        }
    }
}
