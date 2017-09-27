/**
 * Author: Luis R. Gamez
 * Date: September 6th, 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Windows.Forms;

namespace BuyWidget2.Models
{
    /// <summary>
    /// A class that represents a withdrawal aka transfer of Bitcoins
    /// from Octagon to a client's Bitcoin Wallet
    /// </summary>
    public class Signature
    {
        // Declaring var's
        private long nonce { get; set;}                            // Date
        private static string key { get; set;}                     // API Key
        private static string secret { get; set;}                  // Secret API key
        private static string userID { get; set;}                  // User ID
        private string signature { get; set;}                      // Digital signature
        private double amount { get; set;}                         // BTC Amount. Min 0.00006000
        private string address { get; set;}                        // Address to make deposit 
        private int instant { get; set;}                           // Instant tranfer, 0 (false), 1 (true)
        private Dictionary<string, string> values { get; set; }    // Data to be sent to POST Bitstamp API
        private string responseString { get; set; }                 // Response from API

        /// <summary>
        /// Default Constructor.
        /// Use for real time testing ONLY
        /// amount & address already predefined
        /// </summary>
        public Signature()
        {
            nonce = DateTime.UtcNow.Ticks;                             
            key = "rxrmaOmsg9bvEl6dxYBU3ZefNsz8Focd";                 
            secret = "WpHHYqmoq5v9mqRb9pt9NehjEz4hkIKH";             
            userID = "isgr4867";                                      
            signature = GetSignature(nonce, key, secret, userID);     
            amount = 0.00006000;                                       
            address = "15JyA6qsV5fUZaLHJLNT5a87ymV53EjaDD";            
            instant = 0;
            values = new Dictionary<string, string>
            {
                {"key", key},
                {"signature", signature},
                {"nonce", nonce.ToString()},
                {"amount", amount.ToString()},
                {"address", address},
                {"instant", instant.ToString() }
            };
            responseString = GetResponseString(values);
        }

        /// <summary>
        /// Default Constructor.
        /// Use for real time testing ONLY
        /// address already predefined
        /// </summary>
        /// <param name="BitcoinAmount">Bitcoin Amount</param>
        public Signature(double BitcoinAmount)
        {
            nonce = DateTime.UtcNow.Ticks;
            key = "rxrmaOmsg9bvEl6dxYBU3ZefNsz8Focd";
            secret = "WpHHYqmoq5v9mqRb9pt9NehjEz4hkIKH";
            userID = "isgr4867";
            signature = GetSignature(nonce, key, secret, userID);
            amount = BitcoinAmount;
            address = "15JyA6qsV5fUZaLHJLNT5a87ymV53EjaDD";
            instant = 0;
            values = new Dictionary<string, string>
            {
                {"key", key},
                {"signature", signature},
                {"nonce", nonce.ToString()},
                {"amount", amount.ToString()},
                {"address", address},
                {"instant", instant.ToString() }
            };
            responseString = GetResponseString(values);
        }

        /**
         * Get Response from API
         */
        public string getResponseString()
        {
            return responseString;
        }

        /// <summary>
        /// Constructor for deployment only
        /// Amount and address given by the client
        /// </summary>
        /// <param name="BitcoinAmount">the amount of Bitount to purchase</param>
        /// <param name="BitcoinWallet">the Bitcoun wallet address</param>
        public Signature(double BitcoinAmount, string BitcoinWallet)
        {
            nonce = DateTime.UtcNow.Ticks;
            key = "rxrmaOmsg9bvEl6dxYBU3ZefNsz8Focd";
            secret = "WpHHYqmoq5v9mqRb9pt9NehjEz4hkIKH";
            userID = "isgr4867";
            signature = GetSignature(nonce, key, secret, userID);
            amount = BitcoinAmount;
            address = BitcoinWallet;
            instant = 0;
            values = new Dictionary<string, string>
            {
                {"key", key},
                {"signature", signature},
                {"nonce", nonce.ToString()},
                {"amount", amount.ToString()},
                {"address", address},
                {"instant", instant.ToString() }
            };
            responseString = GetResponseString(values);
        }

        /****************************************************************************************************/
        /*****************************    HELPER METHODS    *************************************************/
        /****************************************************************************************************/

        /// <summary>
        /// Communicate with Bitstamp API and get a response string
        /// </summary>
        /// <param name="parameters">Data</param>
        /// <returns>API's response</returns>
        [System.Web.Http.HttpPost]
        [AcceptVerbs("GET", "POST")]
        private string GetResponseString(Dictionary<string, string> parameters)
        {
            var httpClient = new HttpClient();

            var response = httpClient.PostAsync("https://www.bitstamp.net/api/bitcoin_withdrawal/", new FormUrlEncodedContent(parameters)).Result;

            var contents = response.Content.ReadAsStringAsync().Result;

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
        /// <param name="key">A key</param>
        /// <param name="data">Data</param>
        /// <returns>An array of bytes</returns>
        public static byte[] SignHMACSHA256(String key, byte[] data)
        {
            HMACSHA256 hashMaker = new HMACSHA256(Encoding.ASCII.GetBytes(key));
            return hashMaker.ComputeHash(data);
        }

        /// <summary>
        /// Creates a byte array fron a String
        /// </summary>
        /// <param name="str">A string</param>
        /// <returns>An Array of Bytes</returns>
        public static byte[] StrinToByteArray(string str)
        {
            return System.Text.Encoding.ASCII.GetBytes(str);
        }

        /// <summary>
        /// Creates a string from a byte array
        /// </summary>
        /// <param name="hash">A hash</param>
        /// <returns>A String</returns>
        public static string ByteArrayToString(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
/*********************** END OF SIGNATURE CLASS *************************************/