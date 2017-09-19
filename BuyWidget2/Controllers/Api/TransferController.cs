/**
 * Author: Luis R. Gamez
 * Date: September 6th, 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BuyWidget2.Models;


namespace BuyWidget2.Controllers.Api
{
    /**
     * An API that transfer Bitcoin(s) from one account to another account
     */
    public class TransferController : ApiController
    {
        //POST /api/transfer
        [System.Web.Http.HttpPost]
        [AcceptVerbs("GET", "POST")]
        public string TrasnferBTC()
        {   
            // Create a Signature Object
            Signature signature = new Signature();

            // View Result. ID if Successful, Error if not
            return signature.getResponseString();
        }

        //POST /api/transfer/1
        [System.Web.Http.HttpPost]
        [AcceptVerbs("GET", "POST")]
        [Route("api/transfer/{BitcoinAmount:double}/")]
        public string TrasnferBTC(double BitcoinAmount)
        {
            // Create a Signature Object
            Signature signature = new Signature(BitcoinAmount);

            // View Result. ID if Successful, Error if not
            return signature.getResponseString();
        }

        //POST /api/transfer/1
        [System.Web.Http.HttpPost]
        [AcceptVerbs("GET", "POST")]
        [Route("api/transfer/{BitcoinAmount:double}/{Address:string}")]
        public string TrasnferBTC(double BitcoinAmount, string Address)
        {
            // Create a Signature Object
            Signature signature = new Signature(BitcoinAmount, Address);

            // View Result. ID if Successful, Error if not
            return signature.getResponseString();
        }

    }
}
/*************************** END OF API *********************************************/
