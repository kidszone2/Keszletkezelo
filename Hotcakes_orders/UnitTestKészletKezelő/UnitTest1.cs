namespace UnitTestKészletKezelő
{
    using Hotcakes_orders_data_reading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CreateButton_ShouldBeNormalButton()
        {
            
            Button result = Helpers.AddCategoryButton(2, "plüss", Button_Click, false);
            Assert.AreEqual(result.Left, 210);
            Assert.AreEqual(result.Text, "plüss");
        }

        [TestMethod]
        public void CreateButton_LastButtonTrue()
        {
            Button result = Helpers.AddCategoryButton(2, "plüss", Button_Click, true);
            Assert.AreEqual("Minden kategória", result.Text);
        }

        [TestMethod]
        public void OrderListByQuantity_ShouldSortByQuantity()
        {

            var unorderedList = new List<Product>()
                 {
                 new Product() { SKU = "1", Product_name = "Product1", Quantity = 5 },
                 new Product() { SKU = "2", Product_name = "Product2", Quantity = 1 },
                 new Product() { SKU = "3", Product_name = "Product3", Quantity = 10 },
                 new Product() { SKU = "4", Product_name = "Product4", Quantity = 3 },
                 };

            var orderedList = Helpers.OrderListByQuantity(unorderedList);

            var finalList = new List<Product>()
            {
            new Product() { SKU = "2", Product_name = "Product2", Quantity = 1 },
                 new Product() { SKU = "4", Product_name = "Product4", Quantity = 3 },
                 new Product() { SKU = "1", Product_name = "Product1", Quantity = 5 },
                 new Product() { SKU = "3", Product_name = "Product3", Quantity = 10 },
            };


            Assert.AreEqual(orderedList.Count, unorderedList.Count);
            Assert.AreEqual(orderedList[0].SKU, finalList[0].SKU);
            Assert.AreEqual(orderedList[1].SKU, finalList[1].SKU);
            Assert.AreEqual(orderedList[2].SKU, finalList[2].SKU);
            Assert.AreEqual(orderedList[3].SKU, finalList[3].SKU);
        }

        [TestMethod]
        public void Filter_ShouldReturnFilteredList()
        {
            var unorderedList = new List<Product>
             {
             new Product {SKU = "1",Product_name = "Product1", Category = "plüss", Quantity = 10},
             new Product {SKU = "2",Product_name = "Product2", Category = "puzzle", Quantity = 5},
             new Product {SKU = "3",Product_name = "Product3", Category = "plüss", Quantity = 2},
             new Product {SKU = "4",Product_name = "Product4", Category = "lego", Quantity = 20},

             };

            var filterCategory = "plüss";

            var finalList = new List<Product>
             {
             new Product {SKU = "1",Product_name = "Product1", Category = "plüss", Quantity = 10},
             new Product {SKU = "3",Product_name = "Product3", Category = "plüss", Quantity = 2},

             };

            var filteredList = Helpers.Filter(unorderedList, filterCategory);

            Assert.AreEqual(filteredList.Count, 2);
            Assert.AreEqual(filteredList[0].SKU, finalList[0].SKU);
            Assert.AreEqual(filteredList[1].SKU, finalList[1].SKU);
        }
        public void Button_Click(object sender, EventArgs e)
        {

        }
    }
}
