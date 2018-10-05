using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ShareTradingAPI.DataAccess.SQLServer
{
    public class ProductsQuery : IProductsQuery
    {
        readonly SQLServerDatabaseConnection _sqlServerDatabaseConnection;
        public ProductsQuery(SQLServerDatabaseConnection sqlServerDatabaseConnection)
        {
            _sqlServerDatabaseConnection = sqlServerDatabaseConnection;
        }

        public async Task<IEnumerable<string>> Evaluate()
        {
            using (var cn = _sqlServerDatabaseConnection.New())
            {
                await cn.OpenAsync();

                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "dbo.usp_GetProducts";

                    using (var dr = await cmd.ExecuteReaderAsync())
                    {
                        var result = new List<string>();
                        while (await dr.ReadAsync())
                        {
                            result.Add((string)dr["ProductCode"]);
                        }
                        return result;
                    }
                }

            }
        }
    }
}
