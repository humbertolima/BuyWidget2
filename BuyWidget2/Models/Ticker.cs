using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuyWidget2.Models
{
    /// <summary>
    /// A class that represents a Cryptocurreny Ticket
    /// </summary>
    public class Ticker
    {
        public string last { get; set; }          // Last BTC Price
        public string high { get; set; }          // Last 24 hours price high
        public string low { get; set; }           // Last 24 hours price low
        public string vwap { get; set; }          // Last 24 hours volumme wighted average price
        public string volume { get; set; }        // Last 24 hours volume
        public string bid { get; set; }           // Highest buyC
        public string ask { get; set; }           // Lowest sell order
        public string timestamp { get; set; }     // Unix timestamp date and time
        public string open { get; set; }          // First price of the day
        private static double fee = 1.50;         // Fee of transaction

        /// <summary>
        /// Get Octagon Price of Bitcoin
        /// </summary> 
        public string getData()
        {
            return "last: $" + OctagonCryptocurrencyPrice() + " high: $" + high;
        }

        /// <summary>
        /// Price Modifier
        /// </summary>
        /// <returns>Octagon Cryptocurrency Price</returns>
        public string OctagonCryptocurrencyPrice()
        {
            return (Double.Parse(last) * fee).ToString();
        }

        /// <summary>
        /// Last Bicoin Prince @ Bitstamp
        /// </summary>
        /// <returns>Bitcoin price</returns>
        public string BitstampCryptocurrencyPrice()
        {
            return last;
        }
    }
}
/************************** END OF CLASS ************************************/