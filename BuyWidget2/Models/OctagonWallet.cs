using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web.Http;
using System.Text;
using System.Web;
using System.Net.Http;
//using System.Web.Script.Serialization;
using System.Web.Script.Serialization;


namespace BuyWidget2.Models
{
    public class OctagonWallet
    {
        private string btc_available { get; set;}
        private string btc_balance { get; set; }
        private string btc_reserved { get; set;} 
        private string btceur_fee { get; set; }
        private string btcusd_fee { get; set; }
        private string eth_available { get; set; }
        private string eth_balance { get; set; }
        private string eth_reserved { get; set; }
        private string ethbtc_fee { get; set; }
        private string etheur_fee { get; set; }
        private string ethusd_fee { get; set; }
        private string eur_available { get; set; }
        private string eur_balance { get; set; }
        private string eur_reserved { get; set; }
        private string eurusd_fee { get; set; }
        private string ltc_available { get; set; }
        private string ltc_balance { get; set; }
        private string ltc_reserved { get; set; }
        private string ltcbtc_fee { get; set; }
        private string ltceur_fee { get; set; }
        private string ltcusd_fee { get; set; }
        private string usd_available { get; set; }
        private string usd_balance { get; set; }
        private string usd_reserved { get; set; }
        private string xrp_available { get; set; }
        private string xrp_balance { get; set; }
        private string xrp_reserved { get; set; }
        private string xrpbtc_fee { get; set; }
        private string xrpeur_fee { get; set;}
        private string xrpusd_fee { get; set; }
        private string response { get; set; }
        private long nonce { get; set; }
        private static string key { get; set; }
        private static string secret { get; set; }
        private static string userID { get; set; }
        private string signature { get; set; }
        private Dictionary<string, string> values { get; set; }
        private string ResponseString { get; set; }

        public OctagonWallet()
        {
            nonce = DateTime.UtcNow.Ticks;
            key = "rxrmaOmsg9bvEl6dxYBU3ZefNsz8Focd";
            secret = "WpHHYqmoq5v9mqRb9pt9NehjEz4hkIKH";
            userID = "isgr4867";
            signature = GetSignature(nonce, key, secret, userID);
            values = new Dictionary<string, string>
            {
                {"key", key},
                {"signature", signature},
                {"nonce", nonce.ToString()}
            };
            ResponseString = GetResponseString(values);
        }

        public string GetResponse()
        {
            return ResponseString;
        }

        public string GetBtcAvailable()
        {
            return btc_balance;
        }


        public string GetEthAvailable()
        {
            return eth_balance;
        }

        public string GetUsd()
        {
            return usd_balance;
        }

        /**
         * Get response from an URL
         * @returns a string containing a reponse
         */
        private string GetResponseString(Dictionary<string, string> parameters)
        {
            
            var httpClient = new HttpClient();

            var response = httpClient.PostAsync("https://www.bitstamp.net/api/v2/balance/", new FormUrlEncodedContent(parameters)).Result;

            var contents = response.Content.ReadAsStringAsync().Result;

            // Initializing a JavaScriptSerializer
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

            // Mapping data to Wallet2 object          
            Wallet2 wallet = (Wallet2)javaScriptSerializer.Deserialize(contents, typeof(Wallet2));

            usd_balance = wallet.getUsd();
            eth_balance = wallet.eth_balance;
            btc_balance = wallet.btc_balance;

            return wallet.getUsd();
        }

        /**
         * Create a signature 
         * @params nonce
         * @params key
         * @params secret
         * @params clientId
         * @returns a signature of string class
         */
        private string GetSignature(long nonce, string key, string secret, string clientId)
        {
            string msg = string.Format("{0}{1}{2}", nonce,
                clientId,
                key);

            return ByteArrayToString(SignHMACSHA256(secret, StrinToByteArray(msg))).ToUpper();
        }

        /**
         * Creates a Hash on SHA-256 format
         * @params key, a key
         * @params data, data
         * @returns a byte array
         */
        public static byte[] SignHMACSHA256(String key, byte[] data)
        {
            HMACSHA256 hashMaker = new HMACSHA256(Encoding.ASCII.GetBytes(key));
            return hashMaker.ComputeHash(data);
        }

        /**
         * Creates a byte array fron a String
         * @params str, a string
         * @returns a byte array
         */
        public static byte[] StrinToByteArray(string str)
        {
            return System.Text.Encoding.ASCII.GetBytes(str);
        }

        /**
         * Creates a string from a byte array
         * @params hash, a hash
         * @return a string
         */
        public static string ByteArrayToString(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}