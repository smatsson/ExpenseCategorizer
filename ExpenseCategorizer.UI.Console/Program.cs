using System.Collections.Generic;
using System.Reflection;
using System.Text;
using ExpenseCategorizer.Core;
using ExpenseCategorizer.Model.CalculationModel;
using ExpenseCategorizer.Model.TransactionModel;
using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ExpenseCategorizer.UI.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args == null || args.Length != 2)
                {
                    var exeName = AppDomain.CurrentDomain.FriendlyName;
                    WriteLine("Usage:");
                    WriteLine("\t {0} [input file path] [category file path]", exeName);
                    WriteLine("Examle:");
                    WriteLine(
                        "\t {0} C:\\Users\\MyName\\Desktop\\input.txt C:\\Users\\MyName\\Desktop\\mycategories.xml",
                        exeName);
                    return;
                }

                var inputFile = File.ReadAllText(args[0], Encoding.UTF8);
                var categoryFile = File.ReadAllText(args[1], Encoding.UTF8);

                var options = GetOptions();

                var categories = new CategoryParser(categoryFile).Categories;
                var transactions = new TransactionParser(inputFile, categories, options).Transactions;

                var result = new Calculator().Calculate(transactions);
                EmptyLine();
                WriteLine("Results:");
                EmptyLine();
                foreach (var item in result)
                {
                    WriteLine("{0}\t{1}", item.Category, item.Total);
                }

                EmptyLine();
                EmptyLine();

                var unknownCategories = transactions.Where(f => f.Categories == null || !f.Categories.Any()).ToArray();

                if (unknownCategories.Any())
                {
                    WriteLine("Could not determine categories for the following transactions:");
                    foreach (var unknown in unknownCategories)
                    {
                        WriteLine("{0} - {1}", unknown.Name, unknown.DateTime.ToString(options.DateTimeFormat));
                    }

                    EmptyLine();
                }

                FileCreator.WriteResultToDisk(result, options.ValueCulture);
                FileCreator.WriteUnknownToDisk(unknownCategories);

                EmptyLine();
                EmptyLine();
            }
            catch (Exception e)
            {
                WriteLine("Error!");
                WriteLine(e.ToString());
            }
            finally
            {
                WriteLine("Press any key to exit.");
                System.Console.ReadKey(false);
            }
        }

        #region Console helpers

        private static void EmptyLine()
        {
            WriteLine("");
        }

        private static void WriteLine(string message)
        {
            WriteLine(message, null);
        }

        private static void WriteLine(string message, params object[] args)
        {
            System.Console.WriteLine(message, args);
        }

        private static TransactionParserOptions GetOptions()
        {
            var options = new TransactionParserOptions();
            WriteLine("Please enter configuration to use when reading input.");
            options.DateTimeColumn = ReadNumber("DateTime column number (default 0): ", 0);
            options.DateTimeFormat = ReadString("DateTime format (default yyyy-MM-dd): ", "yyyy-MM-dd");
            options.NameColumn = ReadNumber("Transaction name column number (default 2): ", 2);
            options.ValueColumn = ReadNumber("Value/amount column number (default 3): ", 3);

            var valueCulture = ReadString("Culture to use when parsing values (default sv-SE): ", "sv-SE");
            options.ValueCulture = new CultureInfo(valueCulture);

            return options;
        }

        private static int ReadNumber(string text, int @default)
        {
            var input = ReadString(text, @default.ToString(CultureInfo.InvariantCulture));
            int result;
            if (!int.TryParse(input, NumberStyles.None, CultureInfo.InvariantCulture, out result))
            {
                throw new Exception(string.Format("Could not read \"{0}\" as an integer.", text));
            }

            return result;
        }

        private static string ReadString(string text, string @default)
        {
            System.Console.Write(text);
            var input = System.Console.ReadLine();
            return string.IsNullOrWhiteSpace(input) ? @default : input;
        }

        #endregion

    }
}
