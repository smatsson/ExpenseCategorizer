using System.Collections;
using System.Collections.Generic;

namespace ExpenseCategorizer.Model
{
    public class CategoryCollection : IEnumerable<Category>
    {
        private readonly List<Category> _list;

        public CategoryCollection()
        {
            _list = new List<Category>();
        }

        public string FindCategory(string value)
        {
            return null;
        }

        public IEnumerator<Category> GetEnumerator()
        {
            foreach (var category in _list)
            {
                yield return category;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
