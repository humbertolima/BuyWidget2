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