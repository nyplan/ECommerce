using ECommerce.Application.Repositories;
using ECommerce.Persistence.Contexts;
using n = ECommerce.Domain.Entities;


namespace ECommerce.Persistence.Repositories
{
    internal class InvoiceFileWriteRepository : WriteRepository<n.InvoiceFile>, IInvoiceFileWriteRepository
    {
        public InvoiceFileWriteRepository(ECommerceDbContext context) : base(context)
        {
        }
    }
}
