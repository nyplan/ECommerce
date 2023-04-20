using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities.Common;
using ECommerce.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ECommerce.Persistence.Repositories
{
    public class ReadRepository<TEntity> : IReadRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly ECommerceDbContext _context;
        public ReadRepository(ECommerceDbContext context)
        {
            _context = context;
        }

        public DbSet<TEntity> Table => _context.Set<TEntity>();

        public IQueryable<TEntity> GetAll(bool tracking = true)
            => tracking ? Table : Table.AsNoTracking();

        public IQueryable<TEntity> GetWhere(Expression<Func<TEntity, bool>> expression, bool tracking = true)
            => tracking ? Table.Where(expression) : Table.Where(expression).AsNoTracking();

        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> expression, bool tracking = true)
            =>  tracking ? 
            await Table.FirstOrDefaultAsync(expression) : 
            await Table.AsNoTracking().FirstOrDefaultAsync(expression);

        public async Task<TEntity> GetByIdAsync(string id, bool tracking = true)
            => tracking ?
            await Table.FindAsync(Guid.Parse(id)) :
            await Table.AsNoTracking().FirstOrDefaultAsync(data => data.Id == Guid.Parse(id)); // Marker Pattern

    }
}
