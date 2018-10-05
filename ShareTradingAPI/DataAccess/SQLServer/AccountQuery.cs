using System;
using System.Collections.Generic;
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

                        var result = new AccountDetails()
                        {
                            AccountName = (string)dr["AccountName"],
                            OpeningCash = (int)dr["OpeningCash"],
                            AccountNumber = accountNumber,
                            Portfolio = new List<Investment>()
                        };

                        if (!await dr.NextResultAsync()) throw new Exception("Call to usp_GetAccount did not return a result set contains the portfolio");

                        while (await dr.ReadAsync())
                        {
                            result.Portfolio.Add(new Investment()
                            {
                                ProductCode = (string)dr["ProductCode"],
                                Quantity = (int)dr["CurrentQuantity"]
                            });
                        }

                        if (!await dr.NextResultAsync()) throw new Exception("Call to usp_GetAccount did not return a result set contains the total money");

                        if (await dr.ReadAsync())
                            result.TotalFromTransactions = (int)dr["TotalFromTransactions"];

                        return result;
                    }
                }

            }
        }
    }
}
