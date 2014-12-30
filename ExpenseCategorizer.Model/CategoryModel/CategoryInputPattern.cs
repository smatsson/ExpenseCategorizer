using System.Text.RegularExpressions;

namespace ExpenseCategorizer.Model.CategoryModel
{
    public class CategoryInputPattern
    {
        public Regex Pattern { get; private set; }

        public CategoryInputPattern(string pattern)
        {
            Pattern = new Regex(pattern);
        }
    }
}
