using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BuyWidget2.Models;

namespace BuyWidget2.Controllers.Api
{

    public class BuyETHController : ApiController
    {
        //POST /api/buyeth/
        [System.Web.Http.HttpPost]
        [AcceptVerbs("GET", "POST")]
        public string MakePurchase()
        {
            // Create a BuyBTC Object
            BuyETH purchase = new BuyETH();

            // View Result. ID if Successful, Error if not
            return purchase.getResponseString();
        }

        //POST /api/buyeth/500/1.2
        /**
         * @params Dollars, Ether value in dollars quoted (bitstamp price)
         * @params BtcPurchaseAmount, Ether purchase amount
         */
        [System.Web.Http.HttpPost]
        [AcceptVerbs("GET", "POST")]
        [Route("api/buyeth/{Dollars:double}/{EthPurchaseAmount:double}")]
        public string MakePurchase(double Dollars, double EthPurchaseAmount /*,ClientBtcWalletAddress*/)
        {
            // Create a OctagonWallet Object
            OctagonWallet wallet = new OctagonWallet();

            // Get Ether Balance in the wallet
            var EthWalletBalance = wallet.GetEthAvailable();

            // Withdraw money from the bank equal to the purchase
            BankWithdrawal Withdrawal = new BankWithdrawal(Dollars.ToString());

            // Make purchase
            BuyETH purchase = new BuyETH(EthPurchaseAmount);

            // While Ether's are not available in wallet, keep cheking until it's available in order to make a transfer 
            while (Double.Parse(EthWalletBalance) < EthPurchaseAmount)
            {
                EthWalletBalance = wallet.GetEthAvailable();
            }

            // Make transfer to user's wallet from octagon's wallet
            // Create a EtherTransfer Object
            EtherTransfer signature = new EtherTransfer(EthPurchaseAmount /*, ClientEthWalletAddress  */);

            // View Result. ID if Successful, Error if not
            return purchase.getResponseString();
        }
    }
}
