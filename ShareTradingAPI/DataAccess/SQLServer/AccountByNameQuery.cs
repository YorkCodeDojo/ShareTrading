using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ShareTradingAPI.ViewModels;

namespace ShareTradingAPI.DataAccess.SQLServer
{
    public class AccountByNameQuery : IAccountByNameQuery
    {
        readonly SQLServerDatabaseConnection _sqlServerDatabaseConnection;
        public AccountByNameQuery(SQLServerDatabaseConnection sqlServerDatabaseConnection)
        {
            _sqlServerDatabaseConnection = sqlServerDatabaseConnection;
        }

        public async Task<AccountDetails> Evaluate(string accountName)
        {
            using (var cn = _sqlServerDatabaseConnection.New())
            {
                await cn.OpenAsync();

                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "dbo.usp_GetAccountByName";
                    cmd.Parameters.Add("@AccountName", SqlDbType.NVarChar, 100).Value = accountName;

                    using (var dr = await cmd.ExecuteReaderAsync())
                    {
                        if (!await dr.ReadAsync()) return null;

                        var result = new AccountDetails()
                        {
                            AccountName = (string)dr["AccountName"],
                            OpeningCash = (int)dr["OpeningCash"],
                            AccountNumber = (Guid)dr["AccountNumber"]
                        };

                        return result;
                    }
                }

            }
        }
    }
}
