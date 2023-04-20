using ECommerce.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Repositories
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        DbSet<TEntity> Table { get; }
    }
}
