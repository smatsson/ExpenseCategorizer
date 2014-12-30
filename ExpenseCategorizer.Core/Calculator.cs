using ExpenseCategorizer.Model.CalculationModel;
using ExpenseCategorizer.Model.TransactionModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpenseCategorizer.Core
{
    public class Calculator
    {
        public CategorySummaryCollection Calculate(TransactionCollection transactions)
        {
            var categoryDict = new Dictionary<string, Dictionary<string, CategoryMonthSummary>>();

            var unknown = new List<Transaction>();

            foreach (var transaction in transactions)
            {
                if (transaction.Categories == null || !transaction.Categories.Any())
                {
                    unknown.Add(transaction);
                    continue;
                }

                foreach (var category in transaction.Categories)
                {
                    if (!categoryDict.ContainsKey(category))
                    {
                        categoryDict.Add(category, new Dictionary<string, CategoryMonthSummary>());
                    }

                    var months = categoryDict[category];

                    var monthKey = string.Format("{0}{1}", transaction.DateTime.Year, transaction.DateTime.Month);

                    if (!months.ContainsKey(monthKey))
                    {
                        months.Add(monthKey, new CategoryMonthSummary
                        {
                            Month = new DateTime(transaction.DateTime.Year, transaction.DateTime.Month, 1)
                        });
                    }

                    var monthSummary = months[monthKey];
                    monthSummary.Total += transaction.Amount;
                }
            }

            var categorySummaryCollection = new CategorySummaryCollection();
            foreach (
                var result in
                    categoryDict.Select(
                        f => new CategorySummary {Category = f.Key, MonthSummaries = f.Value.Values.ToList()}).OrderBy(f => f.Category))
            {
                categorySummaryCollection.Add(result);
            }


            return categorySummaryCollection;
        }
    }
}
