using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuyWidget2.Models
{
    /**
     * A class that represents a Transaction object
     */
    public class Transaction
    {
        public string datetime { get; set; }    // Date of transaction
        public string id { get; set; }          // ID
        public string type { get; set; }        // Type: deposit, withdrawal
        public string usd { get; set; }         // amount in USD
        public string btc { get; set; }         // amount in BTC
        public string btc_usd { get; set; }     // currency
        public string fee { get; set; }         // Bitstamp fee
        public string order_id { get; set; }    // Order ID
    }
}
/********************* End of class *****************************/