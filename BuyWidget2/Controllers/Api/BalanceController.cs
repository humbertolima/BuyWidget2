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
            Balance balance = new Balance();

            return balance.getResponseString();
        }
    }
}
