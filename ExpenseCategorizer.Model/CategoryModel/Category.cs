using System.Collections.Generic;

namespace ExpenseCategorizer.Model.CategoryModel
{
    public class Category
    {
        public string Name { get; set; }
        public IEnumerable<CategoryInputPattern> Patterns { get; set; }
    }
}
