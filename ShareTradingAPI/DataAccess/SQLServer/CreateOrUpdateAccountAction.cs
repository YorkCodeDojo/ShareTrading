using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ShareTradingAPI.ViewModels;

namespace ShareTradingAPI.DataAccess.SQLServer
{
    public class CreateOrUpdateAccountAction : ICreateOrUpdateAccountAction
    {
        readonly SQLServerDatabaseConnection  _sqlServerDatabaseConnection;
        public CreateOrUpdateAccountAction(SQLServerDatabaseConnection sqlServerDatabaseConnection)
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
                    cmd.CommandText = "dbo.usp_CreateOrUpdateAccount";
                    cmd.Parameters.Add("@AccountNumber", SqlDbType.UniqueIdentifier).Value = account.AccountNumber;
                    cmd.Parameters.Add("@AccountName", SqlDbType.NVarChar, 100).Value = account.AccountName;
                    cmd.Parameters.Add("@Cash", SqlDbType.Int).Value = account.Cash;
                    cmd.Parameters.Add("@SharesHeld", SqlDbType.Int).Value = account.SharesHeld;

                    await cmd.ExecuteNonQueryAsync();
                }

            }
        }
    }
}
