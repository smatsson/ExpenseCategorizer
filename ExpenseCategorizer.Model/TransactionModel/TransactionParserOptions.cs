using System.Globalization;

namespace ExpenseCategorizer.Model.TransactionModel
{
    public class TransactionParserOptions
    {
        public int ValueColumn { get; set; }
        public int NameColumn { get; set; }
        public int DateTimeColumn { get; set; }
        public string DateTimeFormat { get; set; }
        public CultureInfo ValueCulture { get; set; }
    }
}
