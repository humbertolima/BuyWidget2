using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BuyWidget2.Models;
using System.IO;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using RestSharp;


namespace BuyWidget2.Controllers
{
    public class TickerController : Controller
    {
        // GET: Ticker
        public ActionResult Index()
        {
            string url = @"https://www.bitstamp.net/api/ticker/";

            var json = new WebClient().DownloadString(url);

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

            Ticker ticker = (Ticker) javaScriptSerializer.Deserialize(json, typeof(Ticker));

            return View(ticker);
        }

        /**
         * Checks the balance of the main account
         */
        public ActionResult AccountBalance()
        {
            var nonce = DateTime.UtcNow.Ticks;
            var key = "rxrmaOmsg9bvEl6dxYBU3ZefNsz8Focd";
            var secret = "WpHHYqmoq5v9mqRb9pt9NehjEz4hkIKH";
            var userID = "isgr4867";
            var signature = GetSignature(nonce, key, secret, userID);

            var values = new Dictionary<string, string>
              {
                  {"key", key},
                  {"signature", signature},
                  {"nonce", nonce.ToString()}
              };

            var string2 = GetResponseString(values);

            return Content(string2.ToString());
        }


        /** 
         * Action that transfer Bitcoins to an specific address (wallet)
         *
         * Request Parameters: 
         * Key = Api Key
         * Signature = Signature
         * Nonce = Nonce
         * amount = Bitcoin Amount
         * address = Bitcoin Address
         * instant = 0 (false) , 1 (true) 
         */
        public ActionResult TransferBTC()
        {
            // Declaring var's
            var nonce = DateTime.UtcNow.Ticks;                             // Date
            var key = "rxrmaOmsg9bvEl6dxYBU3ZefNsz8Focd";                  // API Key
            var secret = "WpHHYqmoq5v9mqRb9pt9NehjEz4hkIKH";               // Secret API key
            var userID = "isgr4867";                                       // User ID
            var signature = GetSignature(nonce, key, secret, userID);      // Digital signature
            var amount = 0.00006000;                                       // BTC Amount. Min 0.00006000
            var address = "15JyA6qsV5fUZaLHJLNT5a87ymV53EjaDD";            // Address to make deposit 
            var instant = 0;                                               // Instant tranfer, 0 (false), 1 (true)

            // Mapping Data 
            var values = new Dictionary<string, string>
            {
                {"key", key},
                {"signature", signature},
                {"nonce", nonce.ToString()},
                {"amount", amount.ToString()},
                {"address", address},
                {"instant", instant.ToString() }
            };

            // Getting response 
            var string2 = GetResponseString(values);

            // View Result
            return Content(string2.ToString());
        }

        /**
         * Get response from an URL
         * @returns a string containing a reponse
         */
        private string GetResponseString(Dictionary<string, string> parameters)
        {
            var httpClient = new HttpClient();


           // var response = httpClient.PostAsync("https://www.bitstamp.net/api/v2/balance/", new FormUrlEncodedContent(parameters)).Result;
            var response = httpClient.PostAsync("https://www.bitstamp.net/api/bitcoin_withdrawal/", new FormUrlEncodedContent(parameters)).Result;

            var contents = response.Content.ReadAsStringAsync().Result;

            return contents;
        }

        /**
         * Creates a Digital Signature based on a HMAC-SHA256
         * @returns a string containing a digital signature
         */
        private string GetSignature(long nonce, string key, string secret, string clientId)
        {
            string msg = string.Format("{0}{1}{2}", nonce,
                clientId,
                key);

            //  string msg = nonce + clientId + key;

             return ByteArrayToString(SignHMACSHA256(secret, StrinToByteArray(msg))).ToUpper();

        }
        public static byte[] SignHMACSHA256(String key, byte[] data)
        {
            HMACSHA256 hashMaker = new HMACSHA256(Encoding.ASCII.GetBytes(key));
            return hashMaker.ComputeHash(data);
        }

        public static byte[] StrinToByteArray(string str)
        {
            return System.Text.Encoding.ASCII.GetBytes(str);
        }

        public static string ByteArrayToString(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", "").ToLower();

        }
             
    }
}