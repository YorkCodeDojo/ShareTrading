using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShareTradingAPI.Queries;
using ShareTradingAPI.ViewModels;

namespace ShareTradingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        readonly IAccountQuery _accountQuery;
        readonly IUpdateAccountAction _updateAccountAction;
        readonly ITransactionQuery _transactionQuery;

        public AccountController(IAccountQuery accountQuery, IUpdateAccountAction updateAccountAction, ITransactionQuery transactionQuery)
        {
            _accountQuery = accountQuery;
            _updateAccountAction = updateAccountAction;
            _transactionQuery = transactionQuery;
        }

        [HttpGet("{accountNumber}")]
        public async Task<ActionResult<AccountDetails>> Get(Guid accountNumber)
        {
            return await _accountQuery.Evaluate(accountNumber);
        }

        [HttpGet("{accountNumber}/Transactions")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions(Guid accountNumber)
        {
            return await _transactionQuery.Evaluate(accountNumber);
        }

        [HttpPost]
        public async Task<ActionResult<AccountDetails>> Post([FromBody] string name)
        {
            var account =  new AccountDetails()
            {
                AccountName = name,
                AccountNumber = Guid.NewGuid(),
                Cash = 10000,
                SharesHeld = 0
            };

            await _updateAccountAction.Execute(account);

            return account;

        }

    }


}
