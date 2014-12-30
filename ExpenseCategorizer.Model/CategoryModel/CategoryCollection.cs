using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ExpenseCategorizer.Model.CategoryModel
{
    public class CategoryCollection : IEnumerable<Category>
    {
        private readonly List<Category> _list;

        public CategoryCollection()
        {
            _list = new List<Category>();
        }

        public void Add(Category category)
        {
            _list.Add(category);
        }

        public IEnumerable<string> FindCategories(string value)
        {
            var result = (from category in _list
                from pattern in category.Patterns
                where pattern.Pattern.IsMatch(value)
                select category.Name).ToArray();

            return result.Any() ? result : null;
        }

        public IEnumerator<Category> GetEnumerator()
        {
           return ((IEnumerable<Category>) _list).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
