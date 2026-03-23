using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feex.Infrastructure.Repository
{
    public interface IMongoDbRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetMany(string filterBy, string filterValue);
        Task<T> GetByIdAsync(string id);
        Task CreateAsync(T entity);
        Task<bool> UpdateAsync(string id, T entity);
        Task<bool> DeleteAsync(string id);
    }
}
