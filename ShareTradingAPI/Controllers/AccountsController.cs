using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShareTradingAPI.DataAccess;
using ShareTradingAPI.ViewModels;

namespace ShareTradingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        readonly IAccountQuery _accountQuery;
        readonly ICreateAccountAction _updateAccountAction;
        readonly ITransactionsForAccountQuery _transactionQuery;

        public AccountsController(IAccountQuery accountQuery, ICreateAccountAction updateAccountAction, ITransactionsForAccountQuery transactionQuery)
        {
            _accountQuery = accountQuery;
            _updateAccountAction = updateAccountAction;
            _transactionQuery = transactionQuery;
        }

        [HttpGet("{accountNumber}")]
        public async Task<ActionResult<AccountDetails>> Get(Guid accountNumber)
        {
            var account = await _accountQuery.Evaluate(accountNumber);
            if (account == null) return NotFound();

            return account;
        }

        [HttpGet("{accountNumber}/Transactions")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions(Guid accountNumber)
        {
            return await _transactionQuery.Evaluate(accountNumber);
        }

        [HttpPost]
        public async Task<ActionResult<AccountDetails>> Post(NewAccountRequest newAccountRequest)
        {
            var account = new AccountDetails()
            {
                AccountName = newAccountRequest.AccountName,
                AccountNumber = Guid.NewGuid(),
                OpeningCash = 10000,
                Portfolio = new List<Investment>(),
                TotalFromTransactions = 0
            };

            await _updateAccountAction.Execute(account);

            return account;

        }

    }


}
