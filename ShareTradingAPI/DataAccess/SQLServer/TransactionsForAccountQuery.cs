using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShareTradingAPI.ViewModels;

namespace ShareTradingAPI.DataAccess.SQLServer
{
    public class TransactionsForAccountQuery : ITransactionsForAccountQuery
    {
        readonly SQLServerDatabaseConnection _sqlServerDatabaseConnection;
        public TransactionsForAccountQuery(SQLServerDatabaseConnection sqlServerDatabaseConnection)
        {
            _sqlServerDatabaseConnection = sqlServerDatabaseConnection;
        }

        async Task<List<Transaction>> ITransactionsForAccountQuery.Evaluate(Guid accountNumber)
        {
            using (var cn = _sqlServerDatabaseConnection.New())
            {
                await cn.OpenAsync();

                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "dbo.usp_GetTransactionsForAccount";
                    cmd.Parameters.Add("@AccountNumber", SqlDbType.UniqueIdentifier).Value = accountNumber;

                    using (var dr = await cmd.ExecuteReaderAsync())
                    {
                        var result = new List<Transaction>();
                        while (await dr.ReadAsync())
                        {
                            result.Add(new Transaction()
                            {
                                AccountNumber = accountNumber,
                                ID = (Guid)dr["ID"],
                                Quantity = (int)dr["Quantity"],
                                Time = (DateTime)dr["DateAndTime"],
                                TotalCost = (int)dr["TotalCost"],
                                UnitCost = (int)dr["UnitCost"]
                            });
                        }
                        return result;
                    }
                }

            }
        }


    }
}
