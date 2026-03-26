using Feex.Core.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feex.Infrastructure.Repository
{
    public interface IDapperRepository
    {
        Task<IEnumerable<T>> GetManyAsync<T>(string query, object parameters = null, DbConnectionSource connectionSource = DbConnectionSource.FEEX);
        Task<T> GetAsync<T>(string query, object parameters, DbConnectionSource connectionSource = DbConnectionSource.FEEX);
        Task<int> InsertAsync(string query, object parameters, DbConnectionSource connectionSource = DbConnectionSource.FEEX);
        Task<int> UpdateAsync(string query, object parameters, DbConnectionSource connectionSource = DbConnectionSource.FEEX);
        Task<int> DeleteAsync(string query, object parameters, DbConnectionSource connectionSource = DbConnectionSource.FEEX);
        Task<int> ExecuteStoredProcedureAsync(string procedureName, object parameters = null, DbConnectionSource connectionSource = DbConnectionSource.FEEX);
    }
}
