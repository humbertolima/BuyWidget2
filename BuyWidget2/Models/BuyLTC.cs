/**
 * Author: Luis R. Gamez
 * Date: September 6th, 2017
 */

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
    /// A class that represents a Purchase of Litecoins
    /// </summary>
    public class BuyLTC
    {
        private long nonce { get; set; }                                 // Date
        private static string key { get; set; }                          // Public Key
        private static string secret { get; set; }                       // Secret Key
        private string userID { get; set; }                              // User ID
        private string signature { get; set; }                           // Electronic Signature
        private double amount { get; set; }                              // Litecoin Amount
        private Dictionary<string, string> values { get; set; }          // Values to be sent to Bitstamp
        private string ResponseString { get; set; }                      // Response String 

        /// <summary>
        /// Default Constructor. Amount predefined
        /// </summary>
        public BuyLTC()
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
        /// Deployment Construtor
        /// </summary>
        /// <param name="LitecoinAmount"> The amount of Litecoin to be purchase</param>
        public BuyLTC(double LitecoinAmount)
        {
            nonce = DateTime.UtcNow.Ticks;
            key = "rxrmaOmsg9bvEl6dxYBU3ZefNsz8Focd";
            secret = "WpHHYqmoq5v9mqRb9pt9NehjEz4hkIKH";
            userID = "isgr4867";
            signature = GetSignature(nonce, key, secret, userID);
            amount = LitecoinAmount;
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
        /// Get Response String
        /// </summary>
        /// <returns>A string</returns>
        public string getResponseString()
        {
            return ResponseString;
        }

        /// <summary>
        /// Send information to Bitstamp's API in order to buy Litecoins
        /// </summary>
        /// <param name="parameters">Parametes to be sent</param>
        /// <returns>Return a string</returns>
        private string GetResponseString(Dictionary<string, string> parameters)
        {
            // Create an HTTP client
            var httpClient = new HttpClient();

            // Get response from API's
            var response = httpClient.PostAsync("https://www.bitstamp.net/api/v2/buy/market/ltcusd/", new FormUrlEncodedContent(parameters)).Result;

            // Get content from response
            var contents = response.Content.ReadAsStringAsync().Result;

            // Return content
            return contents;
        }

        /// <summary>
        /// Get A Signature coded in HMAC-SHA256
        /// </summary>
        /// <param name="nonce">Date or unique number</param>
        /// <param name="key">Public Key</param>
        /// <param name="secret">Secret Key</param>
        /// <param name="clientId">Client ID bound to API's keys</param>
        /// <returns>A string containing a signature</returns>
        private string GetSignature(long nonce, string key, string secret, string clientId)
        {
            string msg = string.Format("{0}{1}{2}", nonce,
                clientId,
                key);

            return ByteArrayToString(SignHMACSHA256(secret, StrinToByteArray(msg))).ToUpper();
        }

        /// <summary>
        /// Creates an array of bytes coded in HMAC-SHA256
        /// </summary>
        /// <param name="key">Secret Key</param>
        /// <param name="data">Data</param>
        /// <returns>An array of bytes</returns>
        public static byte[] SignHMACSHA256(String key, byte[] data)
        {
            HMACSHA256 hashMaker = new HMACSHA256(Encoding.ASCII.GetBytes(key));
            return hashMaker.ComputeHash(data);
        }

        /// <summary>
        /// Turns a string into a array of bytes
        /// </summary>
        /// <param name="str">A string</param>
        /// <returns>An array of bytes</returns>
        public static byte[] StrinToByteArray(string str)
        {
            return System.Text.Encoding.ASCII.GetBytes(str);
        }

        /// <summary>
        /// Turn an array of bytes into a string
        /// </summary>
        /// <param name="hash">An array of bytes</param>
        /// <returns>A string in lower case with no dashes "-"</returns>
        public static string ByteArrayToString(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
/***************************** END OF CLASS ********************************/