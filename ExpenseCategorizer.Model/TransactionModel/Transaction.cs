
using System;
using System.Collections.Generic;

namespace ExpenseCategorizer.Model.TransactionModel
{
    public class Transaction
    {
        public string Name { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Amount { get; set; }
    }
}
