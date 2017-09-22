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
    /**
     * A class that represents a Signature that communicates with
     * Bistamp API
     */
    public class LitecoinTransfer
    {
        // Declaring var's
        private long nonce { get; set; }                            // Date
        private static string key { get; set; }                     // API Key
        private static string secret { get; set; }                  // Secret API key
        private static string userID { get; set; }                  // User ID
        private string signature { get; set; }                      // Digital signature
        private double amount { get; set; }                         // Litecoin Amount. Min 0.00006000
        private string address { get; set; }                        // Address to make deposit 
        private int instant { get; set; }                           // Instant tranfer, 0 (false), 1 (true)
        private Dictionary<string, string> values { get; set; }     // Data to be sent to POST Bitstamp API
        private string responseString { get; set; }                 // Response from API

        /**
         * Default Constructor.
         * Use for real time testing ONLY
         * amount & address already predefined
         */
        public LitecoinTransfer()
        {
            nonce = DateTime.UtcNow.Ticks;
            key = "rxrmaOmsg9bvEl6dxYBU3ZefNsz8Focd";
            secret = "WpHHYqmoq5v9mqRb9pt9NehjEz4hkIKH";
            userID = "isgr4867";
            signature = GetSignature(nonce, key, secret, userID);
            amount = 0.00006000;
            address = "Litecoin Address";
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
         * Default Constructor.
         * Use for real time testing ONLY
         * address already predefined
         */
        public LitecoinTransfer(double litecoinAmount)
        {
            nonce = DateTime.UtcNow.Ticks;
            key = "rxrmaOmsg9bvEl6dxYBU3ZefNsz8Focd";
            secret = "WpHHYqmoq5v9mqRb9pt9NehjEz4hkIKH";
            userID = "isgr4867";
            signature = GetSignature(nonce, key, secret, userID);
            amount = litecoinAmount;
            address = "A litecoin address";
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

        /**
        * Constructor for deployment only
        * Amount and address given by the client
        * @params etherAmount, the amount of Bitount to purchase
        * @params etherWallet, the ether wallet address
        */
        public LitecoinTransfer(double litecoinAmount, string litecoinWallet)
        {
            nonce = DateTime.UtcNow.Ticks;
            key = "rxrmaOmsg9bvEl6dxYBU3ZefNsz8Focd";
            secret = "WpHHYqmoq5v9mqRb9pt9NehjEz4hkIKH";
            userID = "isgr4867";
            signature = GetSignature(nonce, key, secret, userID);
            amount = litecoinAmount;
            address = litecoinWallet;
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

        /**
         * Communicate with Bitstamp API and get a response string
         * @return a string containing the api's reply
         */
        [System.Web.Http.HttpPost]
        [AcceptVerbs("GET", "POST")]
        private string GetResponseString(Dictionary<string, string> parameters)
        {
            // Create Http Client
            var httpClient = new HttpClient();

            // Getting response string from POST Bitstamp API
            var response = httpClient.PostAsync("https://www.bitstamp.net/api/ltc_withdrawal/", new FormUrlEncodedContent(parameters)).Result;

            // Getting content from response
            var contents = response.Content.ReadAsStringAsync().Result;

            // Return content
            return contents;
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
/*********************** END OF SIGNATURE CLASS *************************************/
