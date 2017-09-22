using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuyWidget2.Models
{
    public class TransactionsCollection
    {
        public IEnumerable<Transaction> Transactions { get; set; }
    }
}