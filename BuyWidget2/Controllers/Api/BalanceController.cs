/**
 * Author: Luis R. Gamez
 * Date: September 6th, 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using BuyWidget2.Models;

namespace BuyWidget2.Controllers.Api
{
    public class BalanceController : ApiController
    {
        // GET /api/balance
        /// <summary>
        /// Get Ballance of all currencies in the account
        /// </summary>
        /// <returns>Balances on the account</returns>
        public string GetBalance()
        {
            // Create Balance object to check wallet's balance
            Balance balance = new Balance();

            // get a balance
            return balance.getResponseString();
        }
    }
}
/************************** END OF CLASS ************************************/
