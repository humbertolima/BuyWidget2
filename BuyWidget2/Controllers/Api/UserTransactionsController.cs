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
        public string GetTransactions()
        {
            // Create a Transaction Object
            UserTransactions transaction = new UserTransactions();

            // Return data
            return transaction.getResponseString();
        }
    }
}
/************************** End of Controller *********************************/
