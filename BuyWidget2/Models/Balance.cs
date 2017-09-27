/**
 * Author: Luis R. Gamez
 * Date: September 6th, 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace BuyWidget2.Models
{
    /**
     * A class that represents the balance of a electronic wallet at Bitstamp
     */
    public class Balance
    {
        private long nonce { get; set;}
        private static string key { get; set;}
        private static string secret { get; set;}
        private static string userID { get; set;}
        private string signature { get; set;}
        private Dictionary<string, string> values { get; set;}
        private string ResponseString { get; set;}

        public Balance()
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

        public string getResponseString()
        {
            return ResponseString;
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