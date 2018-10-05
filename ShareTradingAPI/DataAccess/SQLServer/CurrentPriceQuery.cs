using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ShareTradingAPI.DataAccess.SQLServer
{
    public class CurrentPriceQuery : ICurrentPriceQuery
    {
        public static class ErrorConditions
        {
            public const int ProductDoesNotExist = -1;
            public const int PriceDoesNotExist = -2;
        }

        readonly SQLServerDatabaseConnection _sqlServerDatabaseConnection;
        public CurrentPriceQuery(SQLServerDatabaseConnection sqlServerDatabaseConnection)
        {
            _sqlServerDatabaseConnection = sqlServerDatabaseConnection;
        }

        public async Task<int> Evaluate(string productCode, int minutes)
        {
            using (var cn = _sqlServerDatabaseConnection.New())
            {
                await cn.OpenAsync();

                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "dbo.usp_GetProduct";
                    cmd.Parameters.Add("@ProductCode", SqlDbType.VarChar,100).Value = productCode;
                    cmd.Parameters.Add("@Minutes", SqlDbType.Int).Value = minutes;


                    using (var dr = await cmd.ExecuteReaderAsync())
                    {
                        if (!await dr.ReadAsync()) return ErrorConditions.ProductDoesNotExist;

                        if (await dr.IsDBNullAsync(dr.GetOrdinal("Price")))
                        {
                            return ErrorConditions.PriceDoesNotExist;
                        }

                        return (int)dr["Price"];
                    }
                }

            }
        }

    }
}
