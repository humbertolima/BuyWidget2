using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using BuyWidget2.Models;

namespace BuyWidget2.Controllers.Api
{
    public class SellBitcoinController : ApiController
    {
        //POST /api/sellbitcoin/
        [System.Web.Http.HttpPost]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public string SellBtc()
        {
            SellBitcoin sellOrder = new SellBitcoin();
            
            return sellOrder.getResponseString();           
        }

        //POST /api/sellbitcoin/{bitcoin_amount}
        [System.Web.Http.HttpPost]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public string SellBitcoin(string bitcoinAmount)
        {
            // verify transfer (deposit)
            //UserTransactions deposit = new UserTransactions(bitcoinAmount);

            // if transer was recieved, get amount

            // sell amount
            SellBitcoin sellOrder = new SellBitcoin(bitcoinAmount);

            return sellOrder.getResponseString();
        }
    }
}
