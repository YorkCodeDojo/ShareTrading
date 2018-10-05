using System;
using System.Data.SqlClient;

namespace ShareTradingAPI.DataAccess.SQLServer
{
    public class SQLServerDatabaseConnection
    {
        readonly string _connectionString;
        public SQLServerDatabaseConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        internal SqlConnection New() => new SqlConnection(_connectionString);
    }
}
