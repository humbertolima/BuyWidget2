using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuyWidget2.Models
{
    public class Wallet2
    {
        public string btc_available { get; set; }
        public string btc_balance { get; set; }
        public string btc_reserved { get; set; }
        public string btceur_fee { get; set; }
        public string btcusd_fee { get; set; }
        public string eth_available { get; set; }
        public string eth_balance { get; set; }
        public string eth_reserved { get; set; }
        public string ethbtc_fee { get; set; }
        public string etheur_fee { get; set; }
        public string ethusd_fee { get; set; }
        public string eur_available { get; set; }
        public string eur_balance { get; set; }
        public string eur_reserved { get; set; }
        public string eurusd_fee { get; set; }
        public string ltc_available { get; set; }
        public string ltc_balance { get; set; }
        public string ltc_reserved { get; set; }
        public string ltcbtc_fee { get; set; }
        public string ltceur_fee { get; set; }
        public string ltcusd_fee { get; set; }
        public string usd_available { get; set; }
        public string usd_balance { get; set; }
        public string usd_reserved { get; set; }
        public string xrp_available { get; set; }
        public string xrp_balance { get; set; }
        public string xrp_reserved { get; set; }
        public string xrpbtc_fee { get; set; }
        public string xrpeur_fee { get; set; }
        public string xrpusd_fee { get; set; }

        public string getUsd()
        {
           // return "Avalable: $" + btc_balance;
            return btc_balance;
        }
    }
}