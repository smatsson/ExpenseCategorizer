using System;
using System.Linq;
using ExpenseCategorizer.Model.TransactionModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpenseCategorizer.Test.Core
{
    [TestClass]
    public class Calculator
    {
        private TransactionCollection _transactions;

        [TestInitialize]
        public void Setup()
        {
            _transactions = new TransactionCollection
            {
                new Transaction
                {
                    Amount = 100,
                    Categories = new[] {"Mat"},
                    DateTime = DateTime.Now.AddMonths(-1),
                    Name = "Bananer"
                },
                new Transaction
                {
                    Amount = 75,
                    Categories = new[] {"Mat"},
                    DateTime = DateTime.Now.AddMonths(-1),
                    Name = "Potatis"
                },
                new Transaction
                {
                    Amount = 600,
                    Categories = new[] {"Kläder"},
                    DateTime = DateTime.Now.AddMonths(-1),
                    Name = "Skor"
                },
                new Transaction
                {
                    Amount = 50,
                    Categories = new[] {"Mat"},
                    DateTime = DateTime.Now,
                    Name = "Äpplen"
                }
            };
        }

        [TestMethod]
        public void Calculator_Can_Sum_Transactions()
        {
            var calc = new ExpenseCategorizer.Core.Calculator();
            var result = calc.Calculate(_transactions);

            Assert.AreEqual(2, result.Count(), "Number of categories did not match.");

            var mat = result.First(f => f.Category == "Mat");
            var klader = result.First(f => f.Category == "Kläder");

            Assert.AreEqual(2, mat.MonthSummaries.Count());
            Assert.AreEqual(1, klader.MonthSummaries.Count());

            Assert.AreEqual(225, mat.Total);
            Assert.AreEqual(600, klader.Total);

            var firstMatMonth = mat.MonthSummaries.First();
            var secondMatMonth = mat.MonthSummaries.Skip(1).First();

            Assert.AreEqual(175, firstMatMonth.Total);
            Assert.AreEqual(50, secondMatMonth.Total);

            var firstKladerMonth = klader.MonthSummaries.First();
            Assert.AreEqual(600, firstKladerMonth.Total);
        }
    }
}
