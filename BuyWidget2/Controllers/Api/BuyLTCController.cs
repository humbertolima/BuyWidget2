/**
 * Author: Luis R. Gamez
 * Date: September 6th, 2017
 */

using BuyWidget2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BuyWidget2.Controllers.Api
{
    public class BuyLTCController : ApiController
    {

        //POST /api/buyltc/
        [System.Web.Http.HttpPost]
        [AcceptVerbs("GET", "POST")]
        public string MakePurchase()
        {
            // Create a BuyLTC Object
            BuyLTC purchase = new BuyLTC();

            // View Result. ID if Successful, Error if not
            return purchase.getResponseString();
        }

        //POST /api/buyltc/500/1.2
        /**
         * @params Dollars, LTC value in dollars quoted (bitstamp price)
         * @params LtcPurchaseAmount, Litecoins purchase amount
         */
        [System.Web.Http.HttpPost]
        [AcceptVerbs("GET", "POST")]
        [Route("api/buyltc/{Dollars:double}/{LtcPurchaseAmount:double}")]
        public string MakePurchase(double Dollars, double LtcPurchaseAmount /*,ClientLtcWalletAddress*/)
        {
            // Create a OctagonWallet Object
            OctagonWallet wallet = new OctagonWallet();

            // Get Litecoin Balance in the wallet
            var LtcWalletBalance = wallet.GetLtcAvailable();

            // Withdraw money from the bank equal to the purchase
            BankWithdrawal Withdrawal = new BankWithdrawal(Dollars.ToString());

            // Make purchase
            BuyLTC purchase = new BuyLTC(LtcPurchaseAmount);

            // While LTC's are not available in wallet, keep cheking until it's available in order to make a transfer 
            while (Double.Parse(LtcWalletBalance) < LtcPurchaseAmount)
            {
                LtcWalletBalance = wallet.GetLtcAvailable();
            }

            // Make transfer to user's wallet from octagon's wallet
            // Create a EtherTransfer Object
            LitecoinTransfer signature = new LitecoinTransfer(LtcPurchaseAmount /*, ClientEthWalletAddress  */);

            // View Result. ID if Successful, Error if not
            return purchase.getResponseString();
        }
    }
}
/******************************* End of Controller ***********************************************/