using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ExpenseCategorizer.Model.CategoryModel
{
    public class CategoryCollection : IEnumerable<Category>
    {
        private readonly List<Category> _list;

        private readonly Dictionary<string, IEnumerable<string>> _categoryLookupCache;

        public CategoryCollection()
        {
            _list = new List<Category>();
            _categoryLookupCache = new Dictionary<string, IEnumerable<string>>();
        }

        public void Add(Category category)
        {
            _list.Add(category);
        }

        public IEnumerable<string> FindCategories(string value)
        {
            if (_categoryLookupCache.ContainsKey(value))
            {
                return new List<string>(_categoryLookupCache[value]);
            }

            var result = (from category in _list
                from pattern in category.Patterns
                where pattern.Pattern.IsMatch(value)
                select category.Name)
                .Distinct()
                .ToArray();

            var res = result.Length > 0 ? result : new string[0];
            _categoryLookupCache.Add(value, res);
            return res;
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
