using System.Linq.Expressions;
using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities.Common;
using ECommerce.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ECommerce.Persistence.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity 
{
    private readonly ECommerceDbContext _context;
    public Repository(ECommerceDbContext context)
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
    
    
    
    public async Task<bool> AddAsync(TEntity entity)
    {
        EntityEntry<TEntity> entityEntry = await Table.AddAsync(entity);
        return entityEntry.State == EntityState.Added;
    }

    public async Task<bool> AddRangeAsync(List<TEntity> entities)
    {
        await Table.AddRangeAsync(entities);
        return true;
    }

    public bool Remove(TEntity entity)
    {
        EntityEntry<TEntity> entityEntry =  Table.Remove(entity);
        return entityEntry.State == EntityState.Deleted;
    }
    public bool RemoveRange(List<TEntity> entities)
    {
        Table.RemoveRange(entities);
        return true;
    }

    public async Task<bool> RemoveAsync(string id)
    {
        TEntity? entity = await Table.FirstOrDefaultAsync(data => data.Id == Guid.Parse(id));
        return Remove(entity!);
    }

    public bool Update(TEntity entity)
    {
        EntityEntry<TEntity> entityEntry =  Table.Update(entity);
        return entityEntry.State == EntityState.Modified;
    }

    public async Task<int> SaveAsync()
        => await _context.SaveChangesAsync();
   
}