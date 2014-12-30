using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseCategorizer.Model;

namespace ExpenseCategorizer.Core
{
    public class CategoryParser
    {
        private readonly string _categoryXml;
        private readonly Lazy<CategoryCollection> _categories;

        public CategoryCollection Categories
        {
            get { return _categories.Value; }
        }


        public CategoryParser(string categoryXml)
        {
            _categoryXml = categoryXml;
            _categories = new Lazy<CategoryCollection>(ParseCollection);
        }

        private static CategoryCollection ParseCollection()
        {
            throw new NotImplementedException();
        }
    }
}
