/**
 * Author: Luis R. Gamez
 * Date: September 6th, 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BuyWidget2.Models;

namespace BuyWidget2.Controllers.Api
{
    public class BalanceController : ApiController
    {
        // GET /api/balance
        public string GetBalance()
        {
            // Create Balance object to check wallet's balance
            Balance balance = new Balance();

            // get a balance
            return balance.getResponseString();
        }
    }
}
