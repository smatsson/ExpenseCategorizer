using ExpenseCategorizer.Model.CalculationModel;
using ExpenseCategorizer.Model.TransactionModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ExpenseCategorizer.UI.Console
{
    public static class FileCreator
    {
        public static void WriteResultToDisk(CategorySummaryCollection result, CultureInfo culture)
        {
            var allMonths =
                result.SelectMany(f => f.MonthSummaries.Select(m => m.Month))
                    .Distinct()
                    .OrderByDescending(f => f)
                    .ToList();

            using (var sw = File.CreateText(Path.Combine(GetAssemblyDirectory(), "result.csv")))
            {
                // Headers
                sw.WriteLine("Category\tTotal\tAverage\t{0}", string.Join("\t", allMonths.Select(f => f.ToString("yyyy-MM"))));

                // Rows
                foreach (var category in result)
                {
                    var categoryRow = new List<string> { category.Category };

                    categoryRow.Add(category.Total.ToString(culture));
                    categoryRow.Add((category.Total / allMonths.Count).ToString("F", culture));

                    categoryRow.AddRange(
                        allMonths.Select(month => category.MonthSummaries.FirstOrDefault(f => f.Month == month))
                            .Select(
                                dataForMonth =>
                                    dataForMonth == null
                                        ? "0"
                                        : dataForMonth.Total.ToString(culture)));
                    sw.WriteLine(string.Join("\t", categoryRow));
                }
            }

        }

        public static void WriteUnknownToDisk(IEnumerable<Transaction> unknownTransactions)
        {
            using (var sw = File.CreateText(Path.Combine(GetAssemblyDirectory(), "unknown.txt")))
            {
                foreach (var unknown in unknownTransactions.Select(f => f.Name).Distinct())
                {
                    sw.WriteLine(unknown);
                }
            }
        }

        private static string GetAssemblyDirectory()
        {
            return Path.GetDirectoryName(
                Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
        }
    }
}
