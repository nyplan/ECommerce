using ECommerce.Domain.Entities.Common;

namespace ECommerce.Application.Repositories
{
    public interface IWriteRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<bool> AddAsync(TEntity entity);
        Task<bool> AddRangeAsync(List<TEntity> entities);
        bool Remove(TEntity entity);
        bool RemoveRange(List<TEntity> entities);
        Task<bool> RemoveAsync(string id);
        bool Update(TEntity entity);
        Task<int> SaveAsync();
    }
}
