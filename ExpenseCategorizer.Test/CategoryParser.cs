using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpenseCategorizer.Test
{
    [TestClass]
    public class CategoryParser
    {
        [TestMethod]
        public void CategoryParser_Can_Read_Categories()
        {
            const string categoryFile = @"
                        <categories>
                            <category name=""Food"">
                                <input>^ICA</input>
                                <input>Coop</input>
                            </category>
                        </categories>";

            var categories = new Core.CategoryParser(categoryFile).Categories;

            Assert.AreEqual("Food", categories.FindCategory("ICA Kvantum."));
            Assert.AreEqual("Food", categories.FindCategory("Stora huset Coop."));
            Assert.IsNull(categories.FindCategory("Kvantum ICA."));
            Assert.IsNull(categories.FindCategory(" ICA"));
            Assert.IsNull(categories.FindCategory("Willys"));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Invalid_Category_Input_Throws_Exception()
        {
            new Core.CategoryParser("");
            new Core.CategoryParser("a;b;c");
            new Core.CategoryParser(@"
                        <a>
                            <b name=""Food"">
                                <input>^ICA</input>
                                <input>Coop</input>
                            </b>
                        </a>");
        }
    }
}
