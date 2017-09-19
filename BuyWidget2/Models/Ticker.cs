using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuyWidget2.Models
{
    public class Ticker
    {
        public string last { get; set; }          // Last BTC Price
        public string high { get; set; }          // Last 24 hours price high
        public string low { get; set; }           // Last 24 hours price low
        public string vwap { get; set; }          // Last 24 hours volumme wighted average price
        public string volume { get; set; }        // Last 24 hours volume
        public string bid { get; set; }           // Highest buyC:\Users\luisr\documents\visual studio 2017\Projects\BuyWidget2\BuyWidget2\Models\Ticker.cs order
        public string ask { get; set; }           // Lowest sell order
        public string timestamp { get; set; }     // Unix timestamp date and time
        public string open { get; set; }          // First price of the day
        private static double fee = 0.00;         // Fee of transaction

        /**
         * Get all the data in one single string
         */ 
        public string getData()
        {
            return "last: $" + OctagonCryptocurrencyPrice() + " high: $" + high;
        }

        public string OctagonCryptocurrencyPrice()
        {
            return (Double.Parse(last) + fee).ToString();
        }

        public string BitstampCryptocurrencyPrice()
        {
            return last;
        }
    }
}