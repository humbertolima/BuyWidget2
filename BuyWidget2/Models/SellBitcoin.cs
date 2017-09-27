using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace BuyWidget2.Models
{
    /// <summary>
    ///  A class the represents a Bitcoin Sell Order
    /// </summary>
    public class SellBitcoin
    {
        private long nonce { get; set; }                         // Date                  
        private static string key { get; set; }                  // Public Key   
        private static string secret { get; set; }               // Secret Key
        private static string userID { get; set; }               // User ID
        private string signature { get; set; }                   // A Signature
        private Dictionary<string, string> values { get; set; }  // Values for the API
        private string ResponseString { get; set; }              // Repsonse String
        private string Amount { get; set; }                      // Amount of Bitcoin

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SellBitcoin(/*string BitcoinAmount*/)
        {
            nonce = DateTime.UtcNow.Ticks;
            key = "rxrmaOmsg9bvEl6dxYBU3ZefNsz8Focd";
            secret = "WpHHYqmoq5v9mqRb9pt9NehjEz4hkIKH";
            userID = "isgr4867";
            Amount = "1";
            signature = GetSignature(nonce, key, secret, userID);
            values = new Dictionary<string, string>
            {
                {"key", key},
                {"signature", signature},
                {"nonce", nonce.ToString()},
                {"amount", Amount}
            };
            ResponseString = GetResponseString(values);
        }

        /// <summary>
        /// Method Overload
        /// </summary>
        /// <param name="bitcoinAmount">Amount of Bitcoin to sell</param>
        public SellBitcoin(string bitcoinAmount)
        {
            nonce = DateTime.UtcNow.Ticks;
            key = "rxrmaOmsg9bvEl6dxYBU3ZefNsz8Focd";
            secret = "WpHHYqmoq5v9mqRb9pt9NehjEz4hkIKH";
            userID = "isgr4867";
            Amount = bitcoinAmount;
            signature = GetSignature(nonce, key, secret, userID);
            values = new Dictionary<string, string>
            {
                {"key", key},
                {"signature", signature},
                {"nonce", nonce.ToString()},
                {"amount", Amount}
            };
            ResponseString = GetResponseString(values);
        }
        /// <summary>
        /// Get API's Content Reponse
        /// </summary>
        /// <returns> API's content response </returns>
        public string getResponseString()
        {
            return ResponseString;
        }

        /// <summary>
        /// Get response from an URL
        /// </summary>
        /// <param name="parameters"> Parameters to be sent to the API</param>
        /// <returns>Returns the API's content reponse</returns>
        private string GetResponseString(Dictionary<string, string> parameters)
        {
            // Create an HTTP Cliente
            var httpClient = new HttpClient();

            // Get response from API
            var response = httpClient.PostAsync("https://www.bitstamp.net/api/v2/sell/market/btcusd/", new FormUrlEncodedContent(parameters)).Result;

            // Get contents from API
            var contents = response.Content.ReadAsStringAsync().Result;

            // Return Content
            return contents;
        }

        /// <summary>
        /// Create a signature 
        /// </summary>
        /// <param name="nonce">Date</param>
        /// <param name="key">Public Key</param>
        /// <param name="secret">Secret Key</param>
        /// <param name="clientId">Client ID</param>
        /// <returns></returns>
        private string GetSignature(long nonce, string key, string secret, string clientId)
        {
            string msg = string.Format("{0}{1}{2}", nonce,
                clientId,
                key);

            return ByteArrayToString(SignHMACSHA256(secret, StrinToByteArray(msg))).ToUpper();
        }

        /// <summary>
        /// Creates a Hash on SHA-256 format
        /// </summary>
        /// <param name="key">A Key</param>
        /// <param name="data">Data</param>
        /// <returns></returns>
        public static byte[] SignHMACSHA256(String key, byte[] data)
        {
            HMACSHA256 hashMaker = new HMACSHA256(Encoding.ASCII.GetBytes(key));
            return hashMaker.ComputeHash(data);
        }

        /// <summary>
        /// Creates a byte array fron a String
        /// </summary>
        /// <param name="str">A String</param>
        /// <returns>An Array of Byte</returns>
        public static byte[] StrinToByteArray(string str)
        {
            return System.Text.Encoding.ASCII.GetBytes(str);
        }

        /// <summary>
        /// Creates a string from a byte array
        /// </summary>
        /// <param name="hash">A hash of Byte</param>
        /// <returns>A String</returns>
        public static string ByteArrayToString(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
/***************************** END OF CLASS *********************************/