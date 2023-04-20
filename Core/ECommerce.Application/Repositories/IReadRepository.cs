using ECommerce.Domain.Entities.Common;
using System.Linq.Expressions;

namespace ECommerce.Application.Repositories
{
    public interface IReadRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        IQueryable<TEntity> GetAll(bool tracking = true);
        IQueryable<TEntity> GetWhere(Expression<Func<TEntity, bool>> expression, bool tracking = true);
        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> expression, bool tracking = true);
        Task<TEntity> GetByIdAsync(string id, bool tracking = true);
    }
}
