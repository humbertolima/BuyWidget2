using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BuyWidget2.Models;

namespace BuyWidget2.Controllers.Api
{

    public class BuyBTCController : ApiController
    {

        //POST /api/buybtc/1
        [System.Web.Http.HttpPost]
        [AcceptVerbs("GET", "POST")]
        public string MakePurchase()
        {
            // Create a BuyBTC Object
            BuyBTC purchase = new BuyBTC();

            // View Result. ID if Successful, Error if not
            return purchase.getResponseString();
        }

        //POST /api/buybtc/1
        /**
         * @params Dollars, BTC value in dollars quoted (bitstamp price)
         * @params BtcPurchaseAmount, Bitcoins purchase amount
         */
        [System.Web.Http.HttpPost]
        [AcceptVerbs("GET", "POST")]
        [Route("api/buybtc/{Dollars:double}/{BtcAmount:double}")]
        public string MakePurchase(double Dollars, double BtcPurchaseAmount /*,ClientBtcWalletAddress*/)
        {
            // Create a OctagonWallet Object
            OctagonWallet wallet = new OctagonWallet();

            // Get Bitcoin Balance in the wallet
            var BtcWalletBalance = wallet.GetBtcAvailable();

            // Withdraw money from the bank equal to the purchase
            BankWithdrawal Withdrawal = new BankWithdrawal(Dollars.ToString());

            // Make purchase
            BuyBTC purchase = new BuyBTC(Dollars);

            // While BTC's are not available in wallet, keep cheking until it's available in order to make a transfer 
            while (Double.Parse(BtcWalletBalance) < BtcPurchaseAmount)
            {
                BtcWalletBalance = wallet.GetBtcAvailable();
            }

            // Make transfer to user's wallet from octagon's wallet
            // Create a Signature Object
            Signature signature = new Signature(BtcPurchaseAmount /*, ClientBtcWalletAddress  */);

            // View Result. ID if Successful, Error if not
            return purchase.getResponseString();

        }
    }
}
