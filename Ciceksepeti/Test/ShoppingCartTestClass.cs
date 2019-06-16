using Ciceksepeti.Business.Abstract;
using Ciceksepeti.Business.Common;
using Ciceksepeti.Business.Services;
using Ciceksepeti.Data.Abstract;
using Ciceksepeti.Entities;
using Ciceksepeti.Entities.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    public class ShoppingCartTestClass
    {
        public readonly ShoppingCartService MockShoppingCartService;
        public readonly IProductRepository MockProductRepository;
        public readonly IShoppingCartRepository MockShoppingCartRepository;

        public ShoppingCartTestClass()
        {
            // Test metotları genelinde kullanacağımız Product listesi
            var ProductList = new List<Product>
            {
              new Product { Id = ObjectId.Parse("507f1f77bcf86cd799439011"), IsActive=true, Name="15li Gül Demeti", LastUpdateDate=DateTime.Now, Price=60, Stock=10 },
              new Product { Id = ObjectId.Parse("507f1f77bcf86cd799439012"), CreateDate=DateTime.Now, IsActive=true, Name="Lilyum", LastUpdateDate=DateTime.Now, Price=45, Stock=8 },
              new Product { Id = ObjectId.Parse("507f1f77bcf86cd799439013"), CreateDate=DateTime.Now, IsActive=true, Name="Gül", LastUpdateDate=DateTime.Now, Price=16, Stock=3 },
              new Product { Id = ObjectId.Parse("507f1f77bcf86cd799439014"), CreateDate=DateTime.Now, IsActive=true, Name="Teraryum", LastUpdateDate=DateTime.Now, Price=80, Stock=5 },
              new Product { Id = ObjectId.Parse("507f1f77bcf86cd799439015"), CreateDate=DateTime.Now, IsActive=true, Name="Lale", LastUpdateDate=DateTime.Now, Price=22, Stock=2 },
              new Product { Id = ObjectId.Parse("507f1f77bcf86cd799439016"), CreateDate=DateTime.Now, IsActive=true, Name="Orkide", LastUpdateDate=DateTime.Now, Price=52, Stock=6 }

            };

            // Test metotları genelinde kullanacağımız ShoppingCart listesi
            Dictionary<string, int> Product = new Dictionary<string, int>();
            Product.Add("507f1f77bcf86cd799439016", 1);
            var ShoppingCartList = new List<ShoppingCart>
            {
                new ShoppingCart { Id = ObjectId.Parse("507f1f77bcf86cd79943901c"), CreateDate = DateTime.Now, IsActive = true, LastUpdateDate = DateTime.Now, Product = Product, TotalPrice=52 }
            };

            // Mock the Products Repository using Moq
            var mockProductRepository = new Mock<IProductRepository>();
            var mockShoppingCartRepository = new Mock<IShoppingCartRepository>();


            // GetById metodu için setup işlemi
            mockProductRepository.Setup(mr => mr.GetProductById(It.IsAny<ObjectId>())).Returns((ObjectId i) => ProductList.Find(x => x.Id == i));
            mockShoppingCartRepository.Setup(mr => mr.GetShoppingCartById(It.IsAny<ObjectId>())).Returns((ObjectId i) => ShoppingCartList.Find(x => x.Id == i));

            // Insert için setup işlemi
            mockProductRepository.Setup(mr => mr.AddProduct(It.IsAny<Product>())).Callback(
                (Product target) =>
                {
                    ProductList.Add(target);
                });
            mockShoppingCartRepository.Setup(mr => mr.AddShoppingCart(It.IsAny<ShoppingCart>())).Callback(
              (ShoppingCart target) =>
              {
                  ShoppingCartList.Add(target);
              });

            this.MockProductRepository = mockProductRepository.Object;

            var mockShoppingCartService = new Mock<ShoppingCartService>(mockShoppingCartRepository.Object, mockProductRepository.Object);
            this.MockShoppingCartRepository = mockShoppingCartRepository.Object;
            this.MockShoppingCartService = mockShoppingCartService.Object;
        }


        [Test]
        public void Adding_Shopping_Cart_Nonexistent_Product()
        {
            //non-existent product id
            Dictionary<string, int> Product = new Dictionary<string, int>();
            Product.Add("507f1f77bcf86cd799439010", 1);

            ShoppingCart shoppingCart = new ShoppingCart { CreateDate = DateTime.Now, IsActive = true, LastUpdateDate = DateTime.Now, Product = Product };
            var expected = this.MockShoppingCartService.CreateShoppingCart(shoppingCart);

            Assert.IsNotNull(expected); // Test is not null
            Assert.AreEqual(ResultCodes.NoSuchProduct, expected); // test correct object found
        }

        [Test]
        public void Adding_Shopping_Cart_Stock_Inadequate_Product()
        {
            //non-existent product id
            Dictionary<string, int> Product = new Dictionary<string, int>();
            Product.Add("507f1f77bcf86cd799439015", 3);

            ShoppingCart shoppingCart = new ShoppingCart { CreateDate = DateTime.Now, IsActive = true, LastUpdateDate = DateTime.Now, Product = Product };
            var expected = this.MockShoppingCartService.CreateShoppingCart(shoppingCart);

            Assert.IsNotNull(expected); // Test is not null
            Assert.AreEqual(ResultCodes.NotInStock, expected); // test correct object found
        }

        [Test]
        public void Adding_Shopping_Cart_Existent_Product()
        {

            Dictionary<string, int> Product = new Dictionary<string, int>();
            Product.Add("507f1f77bcf86cd799439011", 1);
            ShoppingCart shoppingCart = new ShoppingCart { CreateDate = DateTime.Now, IsActive = true, LastUpdateDate = DateTime.Now, Product = Product };
            var expected = this.MockShoppingCartService.CreateShoppingCart(shoppingCart);

            Assert.IsNotNull(expected); // Test is not null
            Assert.AreEqual(ResultCodes.OK, expected); // test correct object found
        }

        [Test]
        public void Is_The_Remaining_Stock_Of_The_Product_Correct()
        {

            Dictionary<string, int> Product = new Dictionary<string, int>();
            Product.Add("507f1f77bcf86cd799439011", 1);
            ShoppingCart shoppingCart = new ShoppingCart { CreateDate = DateTime.Now, IsActive = true, LastUpdateDate = DateTime.Now, Product = Product };
            var shoppintCartExpected = this.MockShoppingCartService.CreateShoppingCart(shoppingCart);
            var productExpected = this.MockProductRepository.GetProductById(ObjectId.Parse("507f1f77bcf86cd799439011"));

            Assert.IsNotNull(productExpected); // Test is not null
            Assert.AreEqual(9, productExpected.Stock); // test correct object found
        }

        [Test]
        public void Is_Right_Total_Price()
        {

            Dictionary<string, int> Product = new Dictionary<string, int>();
            Product.Add("507f1f77bcf86cd799439011", 1);
            Product.Add("507f1f77bcf86cd799439012", 1);
            Product.Add("507f1f77bcf86cd799439013", 1);
            ShoppingCart shoppingCart = new ShoppingCart { Id = ObjectId.Parse("507f1f77bcf86cd79943901b"), CreateDate = DateTime.Now, IsActive = true, LastUpdateDate = DateTime.Now, Product = Product };
            this.MockShoppingCartService.CreateShoppingCart(shoppingCart);
            var expected = this.MockShoppingCartRepository.GetShoppingCartById(ObjectId.Parse("507f1f77bcf86cd79943901b"));

            Assert.IsNotNull(expected); // Test is not null
            Assert.AreEqual(121, expected.TotalPrice); // test correct object found
        }

        [Test]
        public void Update_Shopping_Cart_Total_Price()
        {

            Dictionary<string, int> Product = new Dictionary<string, int>();
            Product.Add("507f1f77bcf86cd799439015", 1);
            ShoppingCart shoppingCart = new ShoppingCart { Id = ObjectId.Parse("507f1f77bcf86cd79943901c"), LastUpdateDate = DateTime.Now, Product = Product };
            this.MockShoppingCartService.UpdateShoppingCart(shoppingCart);
            var expected = this.MockShoppingCartRepository.GetShoppingCartById(ObjectId.Parse("507f1f77bcf86cd79943901c"));

            Assert.IsNotNull(expected); // Test is not null
            Assert.AreEqual(74, expected.TotalPrice); // test correct object found
        }

        [Test]
        public void Update_Shopping_Cart_Same_Product_Stock_Control()
        {

            Dictionary<string, int> Product = new Dictionary<string, int>();
            Product.Add("507f1f77bcf86cd799439016", 1);
            ShoppingCart shoppingCart = new ShoppingCart { Id = ObjectId.Parse("507f1f77bcf86cd79943901f"), LastUpdateDate = DateTime.Now, Product = Product };
            this.MockShoppingCartService.CreateShoppingCart(shoppingCart);

            Product.Clear();
            Product.Add("507f1f77bcf86cd799439016", 4);
            shoppingCart = new ShoppingCart { Id = ObjectId.Parse("507f1f77bcf86cd79943901f"), LastUpdateDate = DateTime.Now, Product = Product };
            this.MockShoppingCartService.UpdateShoppingCart(shoppingCart);
            var productExpected = this.MockProductRepository.GetProductById(ObjectId.Parse("507f1f77bcf86cd799439016"));

            Assert.IsNotNull(productExpected); // Test is not null
            Assert.AreEqual(1, productExpected.Stock); // test correct object found
        }
    }
}
