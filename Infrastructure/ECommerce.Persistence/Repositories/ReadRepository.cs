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

        public IQueryable<TEntity> GetAll()
            => Table;

        public IQueryable<TEntity> GetWhere(Expression<Func<TEntity, bool>> expression)
            => Table.Where(expression);

        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> expression)
            => await Table.FirstOrDefaultAsync(expression);


        public async Task<TEntity> GetByIdAsync(string id)
            //=> await Table.FirstOrDefaultAsync(data => data.Id == Guid.Parse(id));  //Marker Pattern
            => await Table.FindAsync(Guid.Parse(id));
    }
}
