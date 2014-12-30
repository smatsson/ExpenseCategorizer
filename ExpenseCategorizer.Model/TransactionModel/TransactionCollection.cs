using System.Collections;
using System.Collections.Generic;

namespace ExpenseCategorizer.Model.TransactionModel
{
    public class TransactionCollection : IEnumerable<Transaction>
    {
        private readonly List<Transaction> _list;

        public TransactionCollection()
        {
            _list = new List<Transaction>();
        }

        public void Add(Transaction transaction)
        {
            _list.Add(transaction);
        }

        public IEnumerator<Transaction> GetEnumerator()
        {
            return ((IEnumerable<Transaction>) _list).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
