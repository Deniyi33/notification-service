using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMI.Infrastructure.DbContexts
{
    public class DapperContext
    {
        private readonly string _connectionString;
        private readonly string _hookMasterConnectionString;
        private readonly string _nx360ConnectionString;


        public DapperContext(string connectionString, string hookMasterConnectionString, string nx360ConnectionString)
        {
            _connectionString = connectionString;
            _hookMasterConnectionString = hookMasterConnectionString;
            _nx360ConnectionString = nx360ConnectionString;
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
        public IDbConnection CreateHookMasterConnection() => new SqlConnection(_hookMasterConnectionString);
        public IDbConnection CreateNX360Connection() => new SqlConnection(_nx360ConnectionString);

    }
}
