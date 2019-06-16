using Ciceksepeti.Data.Abstract;
using Ciceksepeti.Entities;
using Ciceksepeti.Entities.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Ciceksepeti.Data.DbContext;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Ciceksepeti.Data.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository 
    {
        public ProductRepository() : base()
        {

        }

        public void  AddProduct(Product productItem)
        {
            try
            {
                 context.Product.InsertOne(productItem);
            }
            catch (Exception ex)
            {
                //TODO : log or manage exception
                throw ex;
            }
        }

        public Product GetProductById(ObjectId Id)
        {
            try
            {
                return context.Product.Find(model => model.Id == Id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                //TODO : log or manage exceptions
                throw ex;
            }
        }

        public bool UpdateProduct(Product product)
        {
            try
            {
                var actionResult = context.Product.ReplaceOne(
                    p => p.Id.Equals(product.Id), product);
              return  actionResult.IsAcknowledged && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                //TODO : log and manage the exception
                throw ex;
            }
        }
    }
}
