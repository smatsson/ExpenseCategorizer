using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using ExpenseCategorizer.Model.CategoryModel;
using ExpenseCategorizer.Model.TransactionModel;
using System;
using System.Linq;

namespace ExpenseCategorizer.Core
{
    public class TransactionParser
    {
        private readonly Lazy<TransactionCollection> _transactions;
        private readonly TransactionParserOptions _options;
        private readonly string _transactionCsv;
        private readonly CategoryCollection _categories;

        public TransactionCollection Transactions
        {
            get { return _transactions.Value; }
        }

        public TransactionParser(string transactionCsv, CategoryCollection categories, TransactionParserOptions options)
        {

            _transactionCsv = transactionCsv;
            _categories = categories;
            _transactions = new Lazy<TransactionCollection>(ParseTransactions);
            _options = options;
        }

        private TransactionCollection ParseTransactions()
        {
            var transactions = new TransactionCollection();
            var rowCount = 0;
            using (var textReader = new StringReader(_transactionCsv))
            {
                using (var csvReader = new CsvReader(textReader, new CsvConfiguration { Delimiter = "\t", HasHeaderRecord = false}))
                {
                    while (csvReader.Read())
                    {

                        if (IsEmpty(csvReader.CurrentRecord))
                            continue;

                        var transaction = new Transaction
                        {
                            Name = csvReader.GetField<string>(_options.NameColumn)
                        };

                        var valueString = csvReader.GetField<string>(_options.ValueColumn);

                        decimal value;
                        if (!decimal.TryParse(valueString, NumberStyles.Number, _options.ValueCulture, out value))
                        {
                            throw new Exception(string.Format("Could not parse value on row {0}", rowCount));
                        }

                        // Only include expenses.
                        if (value >= 0)
                        {
                            continue;
                        }

                        transaction.Amount = Math.Abs(value);

                        var dateTimeString = csvReader.GetField<string>(_options.DateTimeColumn);

                        DateTime dateTime;
                        if (
                            !DateTime.TryParseExact(dateTimeString, _options.DateTimeFormat, CultureInfo.InvariantCulture,
                                DateTimeStyles.None, out dateTime))
                        {
                            throw new Exception(string.Format("Could not parse datetime on row {0}", rowCount));
                        }

                        transaction.DateTime = dateTime;
                        transaction.Categories = _categories.FindCategories(transaction.Name);
                        transactions.Add(transaction);
                        rowCount++;
                    }
                }
            }

            return transactions;
        }

        private bool IsEmpty(string[] currentRecord)
        {
            return currentRecord.All(x => string.IsNullOrWhiteSpace(x));
        }
    }
}
