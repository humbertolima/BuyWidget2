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
using System.Web.Script.Serialization;
using BuyWidget2.Models;


namespace BuyWidget2.Controllers.Api
{
    /**
     * An API that creates a ticket with the Bitcoin quotoes of the day
     */
    public class TickerController : ApiController
    {
        // GET /api/ticker
        /**
         * Get BTC Pricing Data
         * @return a string containing BTC Data
         */
        public string GetData()
        {
            // API URL
            string url = @"https://www.bitstamp.net/api/ticker/";
            
            // Json Data
            var json = new WebClient().DownloadString(url);

            // Initializing a JavaScriptSerializer
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

            // Mapping date to ticker object
            Ticker ticker = (Ticker)javaScriptSerializer.Deserialize(json, typeof(Ticker));

            // Getting BTC prices
            string data = ticker.getData();

            // Return Quote
            return data;
        }

        // GET /api/ticker/1
        /**
         * Get BTC Pricing Data
         * @param type, 1 = BTC, 2 = ETH, 3 = LTC
         * @return a string containing BTC Data
         */
        [System.Web.Http.HttpGet]
        [Route("api/ticker/{type:int}/")]
        public string GetData(int type)
        {
            // API URL
            string url = "";
            // Data to return
            string data = "";

            if (type == 1)
                url = @"https://www.bitstamp.net/api/ticker/";
            else if (type == 2)
                url = @"https://www.bitstamp.net/api/v2/ticker/ethusd/";
            else if (type == 3)
                url = @"https://www.bitstamp.net/api/v2/ticker/ltcusd/";

            // Json Data
            var json = new WebClient().DownloadString(url);

            // Initializing a JavaScriptSerializer
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

            // Mapping date to ticker object
            Ticker ticker = (Ticker)javaScriptSerializer.Deserialize(json, typeof(Ticker));

            // Getting Bitstamp Cryptocurrency price
            //data = ticker.BitstampCryptocurrencyPrice();
            
            // Getting Octagon Cryptocurrency price
            data = ticker.OctagonCryptocurrencyPrice();

            // Return Quote
            return data.ToString();
        }
    }
}
/*************************** END OF API *********************************************/

