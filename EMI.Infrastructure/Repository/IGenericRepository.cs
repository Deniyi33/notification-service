using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EMI.Domain.Entities;

namespace EMI.Infrastructure.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> InsertAsync(TEntity entity, bool commitTransaction = true);
        Task<List<TEntity>> InsertAsync(List<TEntity> entities, bool commitTransaction = true);
        Task<TEntity> UpdateAsync(TEntity entity, bool commitTransaction = true);
        Task<List<TEntity>> UpdateAsync(List<TEntity> entities, bool commitTransaction = true);
        Task<TEntity> GetByIdAsync(long Id);
        TEntity GetOne(Func<TEntity, bool> predicate);
        Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> GetAllAsync();
        (List<TEntity> records, int totalRecords) Paginate(int page, int pageSize, Func<TEntity, Boolean> predicate);
        IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> GetManyByIdsAsync(IEnumerable<long> ids);
        Task<TEntity> GetWithIncludesAsync(
            Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
