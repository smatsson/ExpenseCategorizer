using System.Collections.Generic;
using System.Linq;

namespace ExpenseCategorizer.Model.CalculationModel
{
    public class CategorySummary
    {
        public string Category { get; set; }
        public IEnumerable<CategoryMonthSummary> MonthSummaries { get; set; }

        public decimal Total
        {
            get { return MonthSummaries.Sum(f => f.Total); }
        }
    }
}
