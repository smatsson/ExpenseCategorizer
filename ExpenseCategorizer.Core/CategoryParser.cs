using ExpenseCategorizer.Model.CategoryModel;
using System;
using System.Linq;
using System.Xml.Linq;

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

        private CategoryCollection ParseCollection()
        {
            var doc = XDocument.Parse(_categoryXml);

            var collection = new CategoryCollection();

            foreach (var element in doc.Descendants("categories").Descendants("category"))
            {
                collection.Add(new Category
                {
                    Name = element.Attribute("name").Value,
                    Patterns = element.Descendants("input").Select(input => new CategoryInputPattern(input.Value))
                });
            }

            return collection;
        }
    }
}
