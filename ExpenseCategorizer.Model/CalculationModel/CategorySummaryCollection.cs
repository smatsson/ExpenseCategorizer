using System.Collections;
using System.Collections.Generic;

namespace ExpenseCategorizer.Model.CalculationModel
{
    public class CategorySummaryCollection : IEnumerable<CategorySummary>
    {
        private readonly List<CategorySummary> _list;

        public CategorySummaryCollection()
        {
            _list = new List<CategorySummary>();
        }

        public void Add(CategorySummary categorySummary)
        {
            _list.Add(categorySummary);
        }

        public IEnumerator<CategorySummary> GetEnumerator()
        {
            return ((IEnumerable<CategorySummary>)_list).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
