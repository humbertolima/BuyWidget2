using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using BuyWidget2.Models;
using System.Security.Cryptography;
using System.Text;

namespace BuyWidget2.Controllers.Api
{
    public class OctagonWalletController : ApiController
    {
        // GET 
        /**
        * Get USD from Octagon Wallet
        * @return a string containing BTC Data
        */
        public string GetData()
        { 
            OctagonWallet wallet = new OctagonWallet();

            return wallet.GetResponse();            
         /*   
            // API URL
            string url = @"https://www.bitstamp.net/api/v2/balance/";

            // Json Data
            var json = new WebClient().DownloadString(url);

            // Initializing a JavaScriptSerializer
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

            // Mapping date to ticker object
            OctagonWallet Wallet = (OctagonWallet)javaScriptSerializer.Deserialize(json, typeof(OctagonWallet));

            // Getting BTC prices
            string data = Wallet.GetUsd().ToString();

            // Return Quote
            return data;
            */
        }
    }
}
