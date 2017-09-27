using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace BuyWidget2.Models
{
    /// <summary>
    /// A class that represents the user's transactions (Sell, Deposit, Withdraw)
    /// </summary>
    public class UserTransactions
    {
        // Declaring var's
        private long nonce { get; set; }                            // Date
        private static string key { get; set; }                     // API Key
        private static string secret { get; set; }                  // Secret API key
        private static string userID { get; set; }                  // User ID
        private string signature { get; set; }                      // Digital signature
        private string offset { get; set; }                         // Skip many transactions, default: 0
        private string limit { get; set; }                          // Limit result to that many transactions, default: 100, max: 1000
        private string sort { get; set; }                           // Sorting by date and time: asc - ascending, desc - descending (default: desc)
        private Dictionary<string, string> values { get; set; }     // Data to be sent to POST Bitstamp API
        private string responseString { get; set; }                 // Response from API
        private List<Transaction> list { get; set; }                // List of transactions
        private string Date { get; set; }                           // Date of transaction
        private string Amount { get; set; }                         // Amount of transaction
        private string Deposit { get; set; }                        // Transaction of type "Deposit"
        private string Withdrawal { get; set; }                     // Transaction of type "Withdrawal"
        private string MarketTrade { get; set; }                    // Transaction of type "MarketTrade"

        /// <summary>
        /// Default Constructor. Amount and Date are set
        /// </summary>
        public UserTransactions()
        {
            Deposit = "0";
            Withdrawal = "1";
            MarketTrade = "2";
            list = new List<Transaction>();
            nonce = DateTime.UtcNow.Ticks;
            key = "rxrmaOmsg9bvEl6dxYBU3ZefNsz8Focd";
            secret = "WpHHYqmoq5v9mqRb9pt9NehjEz4hkIKH";
            userID = "isgr4867";
            offset = "0";
            limit = "100";
            sort = "desc";
            signature = GetSignature(nonce, key, secret, userID);
            values = new Dictionary<string, string>
            {
                {"key", key},
                {"signature", signature},
                {"nonce", nonce.ToString()},
                {"offset", offset},
                {"limit", limit},
                {"sort", sort}
            };
            responseString = GetResponseString(values);
        }

        /// <summary>
        /// Production Constructor
        /// </summary>
        /// <param name="amount">Amount of the transaction</param>
        public UserTransactions(string amount)
        {
            Deposit = "0";
            Withdrawal = "1";
            MarketTrade = "2";
            Date = DateTime.UtcNow.ToString("yyyy-MM-dd"); 
            Amount = amount;
            list = new List<Transaction>();
            nonce = DateTime.UtcNow.Ticks;
            key = "rxrmaOmsg9bvEl6dxYBU3ZefNsz8Focd";
            secret = "WpHHYqmoq5v9mqRb9pt9NehjEz4hkIKH";
            userID = "isgr4867";
            offset = "0";
            limit = "100";
            sort = "desc";
            signature = GetSignature(nonce, key, secret, userID);
            values = new Dictionary<string, string>
            {
                {"key", key},
                {"signature", signature},
                {"nonce", nonce.ToString()},
                {"offset", offset},
                {"limit", limit},
                {"sort", sort}
            };
            responseString = GetResponseString(values);
        }

        /// <summary>
        /// Production Constructor
        /// </summary>
        /// <param name="date"> Same date on which the transaction was made</param>
        /// <param name="amount"> Amount of the transaction</param>
        public UserTransactions(string date, string amount)
        {
            Date = date;
            Amount = amount;
            list = new List<Transaction>();
            nonce = DateTime.UtcNow.Ticks;
            key = "rxrmaOmsg9bvEl6dxYBU3ZefNsz8Focd";
            secret = "WpHHYqmoq5v9mqRb9pt9NehjEz4hkIKH";
            userID = "isgr4867";
            offset = "0";
            limit = "100";
            sort = "desc";
            signature = GetSignature(nonce, key, secret, userID);
            values = new Dictionary<string, string>
            {
                {"key", key},
                {"signature", signature},
                {"nonce", nonce.ToString()},
                {"offset", offset},
                {"limit", limit},
                {"sort", sort}
            };
            responseString = GetResponseString(values);
        }

        /// <summary>
        /// Get Response String
        /// </summary>
        /// <returns></returns>
        public string getResponseString()
        {
            return responseString;
        }

        /// <summary>
        /// Get response from an URL
        /// </summary>
        /// <param name="parameters">a string containing a reponse</param>
        /// <returns></returns>
        private string GetResponseString(Dictionary<string, string> parameters)
        {
            // String containing search's result
            string result = "Deposit not found";

            // Creates a http client
            var httpClient = new HttpClient();

            // Getting response of the api
            var response = httpClient.PostAsync("https://www.bitstamp.net/api/user_transactions/", new FormUrlEncodedContent(parameters)).Result;

            // Getting content of the API's response
            var contents = response.Content.ReadAsStringAsync().Result;

            // Creating a JavaScriptSerializer object
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            // Get transactions and add them to a list
            list = serializer.Deserialize<List<Transaction>>(contents);

            // Traversing the list for an especific transaction of type Deposit
            for (int i = 0; i < list.Count; i++)
            {
                var a = list[i];

                //"2017-09-19 21:41:15"
                /*************** FOR TESTING ONLY **********************/
                if (a.datetime.Contains("2017-08-25") && a.type.Equals(Deposit) && a.btc.Equals("0.00298440"))
                {
                    result = "Deposit of "+ a.btc +" BTC has been confirmed!! Thank you";
                    break;
                }

                /*************** FOR PRODUCTION ONLY ************************/
                /*if (a.datetime.Contains(Date) && a.type.Equals(Deposit) && a.btc.Equals(Amount))
                {
                    result = "Deposit has been confirmed!! Thank you";
                    break;
                }*/
            }
            return result;
            //return contents;
        }

        /// <summary>
        /// Create a signature in HMAC-256
        /// </summary>
        /// <param name="nonce">Date</param>
        /// <param name="key">Public Key</param>
        /// <param name="secret">Secret Key</param>
        /// <param name="clientId">Client ID</param>
        /// <returns>A string coded in HMAC-256</returns>
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
        /// <param name="key">Secret Key</param>
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
        /// <returns> An array of bytes</returns>
        public static byte[] StrinToByteArray(string str)
        {
            return System.Text.Encoding.ASCII.GetBytes(str);
        }

        /// <summary>
        /// Creates a string from a byte array
        /// </summary>
        /// <param name="hash">A Hash of bytes</param>
        /// <returns>A string</returns>
        public static string ByteArrayToString(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}