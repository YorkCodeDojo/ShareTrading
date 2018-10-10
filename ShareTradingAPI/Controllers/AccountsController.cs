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
        readonly IAccountByNameQuery _accountByNameQuery;

        public AccountsController(IAccountQuery accountQuery, ICreateAccountAction updateAccountAction, ITransactionsForAccountQuery transactionQuery, IAccountByNameQuery accountByNameQuery)
        {
            _accountQuery = accountQuery;
            _updateAccountAction = updateAccountAction;
            _transactionQuery = transactionQuery;
            _accountByNameQuery = accountByNameQuery;
        }

        /// <summary>
        /// Gets the details and balance of an account
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        [HttpGet("{accountNumber}")]
        public async Task<ActionResult<AccountDetails>> Get(Guid accountNumber)
        {
            var account = await _accountQuery.Evaluate(accountNumber);
            if (account == null) return NotFound();

            return account;
        }


        /// <summary>
        /// Gets all the transactions for an account
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        [HttpGet("{accountNumber}/Transactions")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions(Guid accountNumber)
        {
            return await _transactionQuery.Evaluate(accountNumber);
        }


        /// <summary>
        /// Creates a new account.  Take note of the AccountNumber returned,  you will need this!
        /// </summary>
        /// <param name="newAccountRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<AccountDetails>> Post(NewAccountRequest newAccountRequest)
        {
            var existingAccount = await _accountByNameQuery.Evaluate(newAccountRequest.AccountName);
            if (existingAccount != null)
            {
                return new BadRequestObjectResult($"An account already exists with the name {newAccountRequest.AccountName}, please choose a different name.");
            }

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
