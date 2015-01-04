using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using ExpenseCategorizer.Model.TransactionModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpenseCategorizer.Test.UI.Console
{
    [TestClass]
    public class FileCreator
    {
        [TestMethod]
        public void Results_Are_Written_To_Disk()
        {
            var result = new ExpenseCategorizer.Core.Calculator().Calculate(new TransactionCollection
            {
                new Transaction
                {
                    Amount = 1,
                    Categories = new[] {"A", "B"},
                    DateTime = new DateTime(1970, 10, 01),
                    Name = "Transaction1"
                },
                new Transaction
                {
                    Amount = 1,
                    Categories = new[] {"A"},
                    DateTime = new DateTime(1970, 10, 01),
                    Name = "Transaction2"
                },
                new Transaction
                {
                    Amount = 1,
                    Categories = new[] {"C"},
                    DateTime = new DateTime(1970, 10, 01),
                    Name = "Transaction3"
                },

                new Transaction
                {
                    Amount = 1,
                    Categories = new[] {"A"},
                    DateTime = new DateTime(1970, 09, 01),
                    Name = "Transaction4"
                },
            });

            var filePath = Path.Combine(GetAssemblyDirectory(), "result.csv");

            try
            {
                ExpenseCategorizer.UI.Console.FileCreator.WriteResultToDisk(result, new CultureInfo("sv-SE"));

                var content = File.ReadAllText(filePath);

                Assert.IsFalse(string.IsNullOrEmpty(content));
                using (var sr = new StringReader(content))
                {
                    // Verify headers.
                    var headers = sr.ReadLine();
                    Assert.AreEqual(ToCsvRow("Category", "Total", "Average", "1970-10", "1970-09"), headers);

                    // Verify rows.
                    Assert.AreEqual(ToCsvRow("A", "3", "1,50", "2", "1"), sr.ReadLine());
                    Assert.AreEqual(ToCsvRow("B", "1", "0,50", "1", "0"), sr.ReadLine());
                    Assert.AreEqual(ToCsvRow("C", "1", "0,50", "1", "0"), sr.ReadLine());
                }

            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [TestMethod]
        public void Unknown_Category_Transactions_Are_Written_To_Disk()
        {
            var unknownTransactions= new TransactionCollection
            {
                new Transaction
                {
                    Amount = 1,
                    Categories = null,
                    DateTime = DateTime.Now,
                    Name = "Transaction1"
                },
                new Transaction
                {
                    Amount = 1,
                    Categories = null,
                    DateTime = DateTime.Now,
                    Name = "Transaction2"
                },
            };

            var filePath = Path.Combine(GetAssemblyDirectory(), "unknown.txt");
            try
            {
                ExpenseCategorizer.UI.Console.FileCreator.WriteUnknownToDisk(unknownTransactions);

                var lines = File.ReadAllLines(filePath);
                var index = 0;
                Assert.AreEqual(unknownTransactions.Count(), lines.Length);
                foreach (var unknownTransaction in unknownTransactions)
                {
                    Assert.AreEqual(unknownTransaction.Name, lines[index]);
                    index++;
                }

            }
            finally
            {
                File.Delete(filePath);
            }

        }

        private static string GetAssemblyDirectory()
        {
            return Path.GetDirectoryName(
                Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
        }

        private static string ToCsvRow(params string[] cells)
        {
            return string.Join("\t", cells);
        }

    }
}
