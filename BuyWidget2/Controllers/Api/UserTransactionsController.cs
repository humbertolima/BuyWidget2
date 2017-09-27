using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BuyWidget2.Models;

namespace BuyWidget2.Controllers.Api
{
    public class UserTransactionsController : ApiController
    {
        // GET /api/transactions/
        /// <summary>
        /// API that verifies a deposit. FOR TEST ONLY
        /// </summary>
        /// <returns></returns>
        public string GetTransactions()
        {
            // Create a Transaction Object
            UserTransactions transaction = new UserTransactions();
            
            // Return data
            return transaction.getResponseString();
        }
        // GET /api/transactions/0.023
        /// <summary>
        /// API that verifies a deposit
        /// </summary>
        /// <param name="amount">Amount of Transaction</param>
        /// <returns>A Message</returns>
        public string GetTransactions(string amount)
        {
            // Create a Transaction Object
            UserTransactions transaction = new UserTransactions(amount);

            return transaction.getResponseString();
        }

    }
}
/************************** End of Controller *********************************/
