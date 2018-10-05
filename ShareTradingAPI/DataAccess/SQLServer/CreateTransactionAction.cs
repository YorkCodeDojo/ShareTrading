using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ShareTradingAPI.ViewModels;

namespace ShareTradingAPI.DataAccess.SQLServer
{
    public class CreateTransactionAction : ICreateTransactionAction
    {
        readonly SQLServerDatabaseConnection  _sqlServerDatabaseConnection;
        public CreateTransactionAction(SQLServerDatabaseConnection sqlServerDatabaseConnection)
        {
            _sqlServerDatabaseConnection = sqlServerDatabaseConnection;
        }
        public async Task Execute(Transaction transaction)
        {
            using (var cn = _sqlServerDatabaseConnection.New())
            {
                await cn.OpenAsync();

                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "dbo.usp_CreateTransaction";
                    cmd.Parameters.Add("@AccountNumber", SqlDbType.UniqueIdentifier).Value = transaction.AccountNumber;
                    cmd.Parameters.Add("@DateAndTime", SqlDbType.DateTime2).Value = transaction.Time;
                    cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = transaction.ID;
                    cmd.Parameters.Add("@ProductCode", SqlDbType.NVarChar, 100).Value = transaction.ProductCode;
                    cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = transaction.Quantity;
                    cmd.Parameters.Add("@TotalValue", SqlDbType.Int).Value = transaction.TotalValue;
                    cmd.Parameters.Add("@UnitPrice", SqlDbType.Int).Value = transaction.UnitPrice;

                    await cmd.ExecuteNonQueryAsync();
                }

            }
        }

  
    }
}
