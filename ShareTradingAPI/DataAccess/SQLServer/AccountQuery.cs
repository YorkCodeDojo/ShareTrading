using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ShareTradingAPI.ViewModels;

namespace ShareTradingAPI.DataAccess.SQLServer
{
    public class AccountQuery : IAccountQuery
    {
        readonly SQLServerDatabaseConnection _sqlServerDatabaseConnection;
        public AccountQuery(SQLServerDatabaseConnection sqlServerDatabaseConnection)
        {
            _sqlServerDatabaseConnection = sqlServerDatabaseConnection;
        }

        public async Task<AccountDetails> Evaluate(Guid accountNumber)
        {
            using (var cn = _sqlServerDatabaseConnection.New())
            {
                await cn.OpenAsync();

                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "dbo.usp_GetAccount";
                    cmd.Parameters.Add("@AccountNumber", SqlDbType.UniqueIdentifier).Value = accountNumber;

                    using (var dr = await cmd.ExecuteReaderAsync())
                    {
                        if (!await dr.ReadAsync()) return null;

                        return new AccountDetails()
                        {
                            AccountName = (string)dr["AccountName"],
                            AccountNumber = accountNumber,
                            Cash = (int)dr["Cash"],
                            SharesHeld = (int)dr["SharesHeld"]
                        };
                    }
                }

            }
        }
    }
}
