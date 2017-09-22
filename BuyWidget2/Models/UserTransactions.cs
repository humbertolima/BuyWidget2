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

        /**
         * Default Constructor.
         */
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

        /**
         * Default Constructor.
         * @params date, the date of transaction
         * @params amount, the amount of the transaction
         */
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

        public string getResponseString()
        {
            return responseString;
        }

        /**
        * Get response from an URL
        * @returns a string containing a reponse
        */
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

            // return result
            return result;
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