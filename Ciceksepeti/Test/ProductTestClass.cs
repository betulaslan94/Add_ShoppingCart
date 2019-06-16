using Ciceksepeti.Data.Abstract;
using Ciceksepeti.Entities.Entities;
using MongoDB.Bson;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    public class ProductTestClass
    {
        public readonly IProductRepository MockProductRepository;

        public ProductTestClass()
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

            // Mock the Products Repository using Moq
            var mockProductRepository = new Mock<IProductRepository>();
            

            // GetById metodu için setup işlemi
            mockProductRepository.Setup(mr => mr.GetProductById(It.IsAny<ObjectId>())).Returns((ObjectId i) => ProductList.Find(x => x.Id == i));

            // Insert için setup işlemi
            mockProductRepository.Setup(mr => mr.AddProduct(It.IsAny<Product>())).Callback(
                (Product target) =>
                {
                    ProductList.Add(target);
                });

            // Test metotlarından erişilebilmesi için global olarak tanımladığımız MockProductRepository'e yukarıdaki setup işlemlerini atıyoruz
            this.MockProductRepository = mockProductRepository.Object;
        }

        [Test]
        public void GetById_Product_Check()
        {
            var actual = new Product { Id = ObjectId.Parse("507f1f77bcf86cd799439011"), IsActive = true, Name = "15li Gül Demeti", LastUpdateDate = DateTime.Now, Price = 60, Stock = 10 };

            var expected = this.MockProductRepository.GetProductById(ObjectId.Parse("507f1f77bcf86cd799439011"));

            Assert.IsNotNull(expected); // Test is not null
            Assert.IsInstanceOf(typeof(Product), expected); // Test type
            Assert.AreEqual(actual.Id, expected.Id); // test correct object found
        }

        [Test]
        public void Insert_Product_Test()
        {
            var actual = new Product { Id = ObjectId.Parse("507f1f77bcf86cd799439017"), IsActive = true, Name = "15li Gül Demeti", LastUpdateDate = DateTime.Now, Price = 60, Stock = 10 };

            this.MockProductRepository.AddProduct(actual);

            var expected = this.MockProductRepository.GetProductById(ObjectId.Parse("507f1f77bcf86cd799439017"));

            Assert.IsNotNull(expected); // Test is not null
            Assert.IsInstanceOf(typeof(Product), expected); // Test type
            Assert.AreEqual(actual.Id, expected.Id); // test correct object found
        }
    }
}
