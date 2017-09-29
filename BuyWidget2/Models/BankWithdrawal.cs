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
    /// <summary>
    /// A class that represents a Bank Withdrawal
    /// </summary>
    public class BankWithdrawal
    {
        private long nonce { get; set; }                             // Nonce
        private static string key { get; set; }                      // Public Key
        private static string secret { get; set; }                   // Secret Key
        private static string userID { get; set; }                   // User ID
        private string signature { get; set; }                       // Signature
        private Dictionary<string, string> values { get; set; }      // Values to be sent
        private string ResponseString { get; set; }                  // Reponse String
        private string Amount { get; set;}                           // The amount to withdraw
        private string Account_Currency { get; set;}                 // USD or EUR
        private string Name { get; set; }                            // Full user or company name
        private string IBAN { get; set; }                            // User or company IBAN aka Account Number
        private string BIC { get; set; }                             // The target bank BIC aka SWIFT
        private string Address { get; set; }                         // User or company address
        private string Postal_Code { get; set; }                     // User or company postal code
        private string City { get; set; }                            // User or company city
        private string Country { get; set; }                         // User or company Country. USE 2 char's Alpha-2-Codes
        private string Type { get; set; }                            // Type of withdrawal request ("sepa" or "international")
        private string BankName { get; set; }                        // Target Bank Name
        private string BankAddress { get; set; }                     // Target Bank Address
        private string BankPostalCode { get; set; }                  // Target bank postal code
        private string BankCity { get; set; }                        // Target bank city
        private string BankCountry { get; set; }                     // Target Bank country. USE 2 char's Alpha-2-Codes
        private string BankCurrency { get; set; }                    // The currency in which the funds should be withdrawn. USD, EUR, etc.
        private string Comment { get; set; }                         // OPTIONAL! Withdrawal comment

        /// <summary>
        /// Default Constructor of BankWithdrawall
        /// </summary>
        /// <param name="amount">Dollars to withdraw from bank acoount</param>
        public BankWithdrawal(string amount)
        {
            nonce = DateTime.UtcNow.Ticks;
            key = "rxrmaOmsg9bvEl6dxYBU3ZefNsz8Focd";
            secret = "WpHHYqmoq5v9mqRb9pt9NehjEz4hkIKH";
            userID = "isgr4867";
            signature = GetSignature(nonce, key, secret, userID);
            Amount = amount;
            Account_Currency = "USD";
            Name = "Odyssey Group";
            IBAN = "Account Number";
            BIC = "";
            Address = "user or company addresd";
            Postal_Code = "User or company postal code";
            City = "User or Company City";
            Country = "US";                                // User or company country
            Type = "international";                        // sepa or international
            BankName = "bank name";                        // Target Bank Name
            BankAddress = "a bank address";                // Target Bank Address
            BankPostalCode = "bank's postal code";         // Target bank postal code
            BankCity = "bank's city";                      // Target bank city
            BankCountry = "bank's country";                // Target Bank country. USE 2 char's Alpha-2-Codes
            BankCurrency = "USD";                          // The currency in which the funds should be withdrawn. USD, EUR, etc.
            Comment = "a coment";                          // OPTIONAL! Withdrawal comment
            values = new Dictionary<string, string>
            {
                {"key", key},
                {"signature", signature},
                {"nonce", nonce.ToString()},
                {"amount", Amount},
                {"account_currency", Account_Currency},
                {"name", Name },
                {"IBAN", IBAN },
                {"BIC", BIC },
                {"address", Address },
                {"postal_code", Postal_Code },
                {"city", City },
                {"country", Country },
                {"type", Type },
                {"bank_name", BankName },
                {"bank_address", BankAddress },                     
                {"bank_postal_code", BankPostalCode },            
                {"bank_city", BankCity},                         
                {"bank_country", BankCountry},                    
                {"currency", BankCurrency},                    
                {"comment", Comment}                         
             };
            ResponseString = GetResponseString(values);
        }

        /// <summary>
        /// Get API's response from Bitstamp
        /// </summary>
        /// <returns></returns>
        public string getResponseString()
        {
            return ResponseString;
        }

        /// <summary>
        /// Get response string with given parameters
        /// </summary>
        /// <param name="parameters">data to be sent</param>
        /// <returns></returns>
        private string GetResponseString(Dictionary<string, string> parameters)
        {
            // Create Http Client
            var httpClient = new HttpClient();

            // Get response from API
            var response = httpClient.PostAsync("https://www.bitstamp.net/api/v2/withdrawal/open/", new FormUrlEncodedContent(parameters)).Result;

            // Get content from API's reponse
            var contents = response.Content.ReadAsStringAsync().Result;

            // Return content
            return contents;
        }

        /// <summary>
        /// Get a signature coded in HMAC-SHA256
        /// </summary>
        /// <param name="nonce">A unitque number a.k.a Date</param>
        /// <param name="key">Public Key</param>
        /// <param name="secret">Secret Key</param>
        /// <param name="clientId">Client ID</param>
        /// <returns>A HMAC-SHA256 Signature</returns>
        private string GetSignature(long nonce, string key, string secret, string clientId)
        {
            string msg = string.Format("{0}{1}{2}", nonce,
                clientId,
                key);

            return ByteArrayToString(SignHMACSHA256(secret, StrinToByteArray(msg))).ToUpper();
        }

        /// <summary>
        /// Creates an array of bytes in HMAC-SHA256
        /// </summary>
        /// <param name="key">A key (secret key)</param>
        /// <param name="data">Data to embbed</param>
        /// <returns></returns>
        public static byte[] SignHMACSHA256(String key, byte[] data)
        {
            HMACSHA256 hashMaker = new HMACSHA256(Encoding.ASCII.GetBytes(key));
            return hashMaker.ComputeHash(data);
        }

        /// <summary>
        /// Turns a given string to an array of bytes
        /// </summary>
        /// <param name="str"></param>
        /// <returns>An array of bytes</returns>
        public static byte[] StrinToByteArray(string str)
        {
            return System.Text.Encoding.ASCII.GetBytes(str);
        }

        /// <summary>
        /// Turns an Array of Bytes into a string
        /// </summary>
        /// <param name="hash">A string of bytes</param>
        /// <returns>A string in lower case with no dashes "-"</returns>
        public static string ByteArrayToString(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
/****************************** END OF CLASS ***************************************/