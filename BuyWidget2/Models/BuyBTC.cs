using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Security.Cryptography;
using System.Net.Http;
using System.Text;

namespace BuyWidget2.Models
{
    /// <summary>
    /// A class that represent a Purchase of Bitcoin
    /// </summary>
    public class BuyBTC
    {
        // Declaring var's
        private long nonce { get; set;}                        // Date
        private static string key { get; set;}                 // Public Key
        private static string secret { get; set;}              // Secret Key
        private string userID { get; set;}                     // User ID
        private string signature { get; set;}                  // Signature
        private double amount { get; set;}                     // Bitcoin Amount
        private Dictionary<string, string> values { get; set;} // Values to be sent to API
        private string ResponseString { get; set; }            // API response String

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BuyBTC()
        {
            nonce = DateTime.UtcNow.Ticks;
            key = "rxrmaOmsg9bvEl6dxYBU3ZefNsz8Focd";
            secret = "WpHHYqmoq5v9mqRb9pt9NehjEz4hkIKH";
            userID = "isgr4867";
            signature = GetSignature(nonce, key, secret, userID);
            amount = 0.00283329; 
            values = new Dictionary<string, string>
             {
                 {"key", key},
                 {"signature", signature},
                 {"nonce", nonce.ToString()},
                 {"amount", amount.ToString()}
             };
            ResponseString = GetResponseString(values);
        }

        /// <summary>
        /// Production Constructor
        /// </summary>
        /// <param name="BitcoinAmount"> Bitcoin Amount to be buy</param>
        public BuyBTC(double BitcoinAmount)
        {
             nonce = DateTime.UtcNow.Ticks;
             key = "rxrmaOmsg9bvEl6dxYBU3ZefNsz8Focd";
             secret = "WpHHYqmoq5v9mqRb9pt9NehjEz4hkIKH";
             userID = "isgr4867";
             signature = GetSignature(nonce, key, secret, userID);
             amount = BitcoinAmount;
             values = new Dictionary<string, string>
             {
                 {"key", key},
                 {"signature", signature},
                 {"nonce", nonce.ToString()},
                 {"amount", amount.ToString()}
             };
            ResponseString = GetResponseString(values);
        }

        public string getResponseString()
        {
            return ResponseString;
        }

        /// <summary>
        /// Get response from an URL
        /// </summary>
        /// <param name="parameters">Data to be sent to API</param>
        /// <returns>API's Response</returns>
        private string GetResponseString(Dictionary<string, string> parameters)
        {
            var httpClient = new HttpClient();

            var response = httpClient.PostAsync("https://www.bitstamp.net/api/v2/buy/market/btcusd/", new FormUrlEncodedContent(parameters)).Result;

            var contents = response.Content.ReadAsStringAsync().Result;
            
            return contents;
        }

        /// <summary>
        /// Create a signature 
        /// </summary>
        /// <param name="nonce">Unique value aka Date</param>
        /// <param name="key">Public Key</param>
        /// <param name="secret">Secret Key</param>
        /// <param name="clientId">Client ID</param>
        /// <returns>A Signature</returns>
        private string GetSignature(long nonce, string key, string secret, string clientId)
        {
            string msg = string.Format("{0}{1}{2}", nonce,
                clientId,
                key);

            return ByteArrayToString(SignHMACSHA256(secret, StrinToByteArray(msg))).ToUpper();
        }

        /// <summary>
        ///  Creates a Hash on SHA-256 format
        /// </summary>
        /// <param name="key"> A Key</param>
        /// <param name="data"> Data</param>
        /// <returns>An array of bytes in HMAC-SHA256</returns>
        public static byte[] SignHMACSHA256(String key, byte[] data)
        {
            HMACSHA256 hashMaker = new HMACSHA256(Encoding.ASCII.GetBytes(key));
            return hashMaker.ComputeHash(data);
        }

        /// <summary>
        /// Creates a byte array fron a String
        /// </summary>
        /// <param name="str">a string of characters</param>
        /// <returns>An array of bytes</returns>
        public static byte[] StrinToByteArray(string str)
        {
            return System.Text.Encoding.ASCII.GetBytes(str);
        }

        /// <summary>
        /// Creates a string from a byte array
        /// </summary>
        /// <param name="hash">a hash of bytes</param>
        /// <returns>a string</returns>
        public static string ByteArrayToString(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
/********************** END OF CLASS ****************************************/