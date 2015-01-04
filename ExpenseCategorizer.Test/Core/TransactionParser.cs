using System;
using System.Globalization;
using System.Linq;
using ExpenseCategorizer.Model.CategoryModel;
using ExpenseCategorizer.Model.TransactionModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpenseCategorizer.Test.Core
{
    [TestClass]
    public class TransactionParser
    {

        private const string TransactionCsv = @"2014-12-27	2014-12-27	Kattförsäkring	-500	13 048,05
2014-12-27	2014-12-27	Bil fasta kostnader	-360	13 548,05
2014-12-26	2014-12-26	Hemförsäkring	-131	14 063,05
2014-12-25	2014-12-25	Lön	100	14 194,05";

        private readonly TransactionParserOptions _parserOptions = new TransactionParserOptions
        {
            DateTimeColumn = 0,
            NameColumn = 2,
            ValueColumn = 3,
            DateTimeFormat = "yyyy-MM-dd",
            ValueCulture = new CultureInfo("sv-SE")
        };

        private const string CategoryFile = @"
                        <categories>
                            <category name=""Bil"">
                                <input>^Bil </input>
                            </category>
                            <category name=""Katter"">
                                <input>^Katt</input>
                            </category>
                            <category name=""Försäkring"">
                                <input>Hemförsäkring</input>
                                <input>Bilförsäkring</input>
                            </category>
                        </categories>";

        [TestMethod]
        public void TransactionParser_Can_Parse_Transactions()
        {
            var categoryParser = new ExpenseCategorizer.Core.CategoryParser(CategoryFile);
            var transactionParser = new ExpenseCategorizer.Core.TransactionParser(TransactionCsv, categoryParser.Categories, _parserOptions);
            Assert.AreEqual(3, transactionParser.Transactions.Count());

            var first = transactionParser.Transactions.First();
            var second = transactionParser.Transactions.Skip(1).First();
            var third = transactionParser.Transactions.Skip(2).First();

            AssertTransaction(new Transaction
            {
                Amount = 500,
                Categories = new[] { "Katter" },
                DateTime = new DateTime(2014, 12, 27),
                Name = "Kattförsäkring"
            }, first);

            AssertTransaction(new Transaction
            {
                Amount = 360,
                Categories = new[] { "Bil" },
                DateTime = new DateTime(2014, 12, 27),
                Name = "Bil fasta kostnader"
            }, second);

            AssertTransaction(new Transaction
            {
                Amount = 131,
                Categories = new[] { "Försäkring" },
                DateTime = new DateTime(2014, 12, 26),
                Name = "Hemförsäkring"
            }, third);
        }

        [TestMethod]
        public void TransactionParser_Supports_Different_Columns()
        {
            var categories = new CategoryCollection
            {
                new Category
                {
                    Name = "TestCategory",
                    Patterns = new[] {new CategoryInputPattern("name")}
                }
            };

            var transactionParser = new ExpenseCategorizer.Core.TransactionParser("name\t-100\t2014-10-27", categories, new TransactionParserOptions
            {
                DateTimeColumn = 2,
                NameColumn = 0,
                ValueColumn = 1,
                DateTimeFormat = "yyyy-MM-dd",
                ValueCulture = new CultureInfo("sv-SE")
            });

            Assert.AreEqual(1, transactionParser.Transactions.Count(), "Transaction count does not match.");
            var first = transactionParser.Transactions.First();

            AssertTransaction(new Transaction
            {
                Amount = 100,
                Categories = new[] { "TestCategory" },
                DateTime = new DateTime(2014, 10, 27),
                Name = "name"
            }, first);

        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void InvalidData_Throws_Exception()
        {
            var categories = new ExpenseCategorizer.Core.CategoryParser(CategoryFile).Categories;
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable once NotAccessedVariable
            var a = new ExpenseCategorizer.Core.TransactionParser("", categories, _parserOptions).Transactions;
            // Invalid separator
            // ReSharper disable RedundantAssignment
            a = new ExpenseCategorizer.Core.TransactionParser("a;b;c", categories, _parserOptions).Transactions;

            // Invalid number of columns
            a = new ExpenseCategorizer.Core.TransactionParser("a\tb\tc", categories, _parserOptions).Transactions;
            a = new ExpenseCategorizer.Core.TransactionParser("<first>a</first>", categories, _parserOptions).Transactions;
            // Invalid datetime format.
            a =
                new ExpenseCategorizer.Core.TransactionParser("2014/12/27	2014-12-27	Kattförsäkring	-500", categories, _parserOptions)
                    .Transactions;
            // ReSharper restore RedundantAssignment
            // ReSharper restore ObjectCreationAsStatement
        }

        [TestMethod]
        public void Transactions_With_Unknown_Categories_Have_Categories_Set_To_Null()
        {
            var categories = new ExpenseCategorizer.Core.CategoryParser(CategoryFile).Categories;
            var transactions = new ExpenseCategorizer.Core.TransactionParser("2014-12-27	2014-12-27	Other	-500	13 048,05", categories,
                _parserOptions).Transactions;

            Assert.AreEqual(1, transactions.Count(), "Transaction count does not match.");
            AssertTransaction(new Transaction
            {
                Amount = 500,
                Categories = null,
                DateTime = new DateTime(2014, 12, 27),
                Name = "Other"
            }, transactions.First());
        }

        private static void AssertTransaction(Transaction expected, Transaction actual)
        {
            Assert.AreEqual(expected.Amount, actual.Amount, "Amount does not match.");


            if (expected.Categories == null)
            {
                Assert.IsTrue(actual.Categories == null);
            }
            else
            {
                var expectedCategories = expected.Categories.ToArray();
                var actualCategories = actual.Categories.ToArray();

                Assert.AreEqual(expectedCategories.Count(), actualCategories.Count(), "Category count does not match.");

                for (var i = 0; i < expectedCategories.Length; i++)
                {
                    Assert.AreEqual(expectedCategories[i], actualCategories[i]);
                }
            }

            Assert.AreEqual(expected.DateTime, actual.DateTime, "DateTime does not match.");
            Assert.AreEqual(expected.Name, actual.Name, "Name does not match.");
        }

    }
}
