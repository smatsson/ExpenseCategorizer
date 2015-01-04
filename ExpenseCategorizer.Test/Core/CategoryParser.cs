using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpenseCategorizer.Test.Core
{
    [TestClass]
    public class CategoryParser
    {
        private const string CategoryFile = @"
                        <categories>
                            <category name=""Food"">
                                <input>^ICA</input>
                                <input>Coop</input>
                            </category>
                        </categories>";

        [TestMethod]
        public void CategoryParser_Can_Read_Categories()
        {
            var categories = new ExpenseCategorizer.Core.CategoryParser(CategoryFile).Categories;

            Assert.AreEqual("Food", categories.FindCategories("ICA Kvantum.").First());
            Assert.AreEqual("Food", categories.FindCategories("Stora huset Coop.").First());
            Assert.IsNull(categories.FindCategories("Kvantum ICA."));
            Assert.IsNull(categories.FindCategories(" ICA"));
            Assert.IsNull(categories.FindCategories("Willys"));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void Invalid_Category_Input_Throws_Exception()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable once NotAccessedVariable
            var a = new ExpenseCategorizer.Core.CategoryParser("").Categories;
            // ReSharper disable RedundantAssignment
            a = new ExpenseCategorizer.Core.CategoryParser("a;b;c").Categories;
            a = new ExpenseCategorizer.Core.CategoryParser(@"
                        <a>
                            <b name=""Food"">
                                <input>^ICA</input>
                                <input>Coop</input>
                            </b>
                        </a>").Categories;
            // ReSharper restore RedundantAssignment
            // ReSharper restore ObjectCreationAsStatement
        }

        [TestMethod]
        public void Categories_Can_Be_Iterated()
        {
            var index = 0;
            var patterns = new[] { "^ICA", "Coop" };

            var categories = new ExpenseCategorizer.Core.CategoryParser(CategoryFile).Categories;
            Assert.AreEqual(1, categories.Count());
            Assert.AreEqual("Food", categories.First().Name);
            foreach (var input in categories.First().Patterns)
            {
                Assert.AreEqual(patterns[index], input.Pattern.ToString());
                index++;
            }
        }
    }
}
