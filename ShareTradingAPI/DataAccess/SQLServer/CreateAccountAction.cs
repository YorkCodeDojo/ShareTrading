using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ShareTradingAPI.ViewModels;

namespace ShareTradingAPI.DataAccess.SQLServer
{
    public class CreateAccountAction : ICreateAccountAction
    {
        readonly SQLServerDatabaseConnection  _sqlServerDatabaseConnection;
        public CreateAccountAction(SQLServerDatabaseConnection sqlServerDatabaseConnection)
        {
            _sqlServerDatabaseConnection = sqlServerDatabaseConnection;
        }
        public async Task Execute(AccountDetails account)
        {
            using (var cn = _sqlServerDatabaseConnection.New())
            {
                await cn.OpenAsync();

                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "dbo.usp_CreateAccount";
                    cmd.Parameters.Add("@AccountNumber", SqlDbType.UniqueIdentifier).Value = account.AccountNumber;
                    cmd.Parameters.Add("@AccountName", SqlDbType.NVarChar, 100).Value = account.AccountName;
                    cmd.Parameters.Add("@OpeningCash", SqlDbType.Int).Value = account.OpeningCash;

                    await cmd.ExecuteNonQueryAsync();
                }

            }
        }
    }
}
