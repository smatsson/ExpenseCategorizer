using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseCategorizer.Model
{
    public class Category
    {
        public string Name { get; set; }
        public IEnumerable<CategoryInputPattern> Patterns { get; set; }
    }
}
