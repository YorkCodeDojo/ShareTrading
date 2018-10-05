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
    public class AccountController : ControllerBase
    {
        readonly IAccountQuery _accountQuery;
        readonly ICreateOrUpdateAccountAction _updateAccountAction;
        readonly ITransactionsForAccountQuery _transactionQuery;

        public AccountController(IAccountQuery accountQuery, ICreateOrUpdateAccountAction updateAccountAction, ITransactionsForAccountQuery transactionQuery)
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
                Cash = 10000,
                SharesHeld = 0
            };

            await _updateAccountAction.Execute(account);

            return account;

        }

    }


}
