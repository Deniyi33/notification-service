using Dapper;
using EMI.Domain.Enums;
using EMI.Domain;
using EMI.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMI.Infrastructure.DbContexts;
using EMI.Infrastructure.Repository;
using EMI.Infrastructure.DbContexts;
using Microsoft.Data.SqlClient;

namespace EMI.Infrastructure.Repository
{

    public class DapperRepository : IDapperRepository
    {
        private DapperContext _context;
        public DapperRepository(DapperContext context)
        {
            _context = context;
        }

        public IDbConnection GetContextConnection(DbConnectionSource connectionSource = DbConnectionSource.FEEX)
        {
            if (connectionSource == DbConnectionSource.FEEX)
            {
                return _context.CreateConnection();
            }
            else if (connectionSource == DbConnectionSource.HOOKMASTER)
            {
                return _context.CreateHookMasterConnection();
            }
            else if (connectionSource == DbConnectionSource.NX360)
            {
                return _context.CreateNX360Connection();
            }
            return _context.CreateConnection();
        }

        public async Task<IEnumerable<T>> GetManyAsync<T>(string query, object parameters = null, DbConnectionSource connectionSource = DbConnectionSource.FEEX)
        {
            using (var connection = GetContextConnection(connectionSource))
            {
                return await connection.QueryAsync<T>(query, parameters);
            }
        }

        public async Task<T> GetAsync<T>(string query, object parameters, DbConnectionSource connectionSource = DbConnectionSource.FEEX)
        {
            using (var connection = GetContextConnection(connectionSource))
            {
                return await connection.QueryFirstOrDefaultAsync<T>(query, parameters);
            }
        }

        public async Task<int> InsertAsync(string query, object parameters, DbConnectionSource connectionSource = DbConnectionSource.FEEX)
        {
            using (var connection = GetContextConnection(connectionSource))
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<int> UpdateAsync(string query, object parameters, DbConnectionSource connectionSource = DbConnectionSource.FEEX)
        {
            using (var connection = GetContextConnection(connectionSource))
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<int> DeleteAsync(string query, object parameters, DbConnectionSource connectionSource = DbConnectionSource.FEEX)
        {
            using (var connection = GetContextConnection(connectionSource))
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<int> ExecuteStoredProcedureAsync(string procedureName, object parameters = null, DbConnectionSource connectionSource = DbConnectionSource.FEEX)
        {
            using (var connection = GetContextConnection(connectionSource))
            {
                return await connection.ExecuteAsync(
                    procedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }
    }
}
