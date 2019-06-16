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
    public class ShoppingCartRepository : RepositoryBase<ShoppingCart>, IShoppingCartRepository
    {
        public ShoppingCartRepository() : base()
        {

        }
        public void AddShoppingCart(ShoppingCart item)
        {
            try
            {
                context.ShoppingCart.InsertOne(item);
            }
            catch (Exception ex)
            {
                //TODO : log or manage exception
                throw ex;
            }
        }

        public ShoppingCart GetShoppingCartById(ObjectId Id)
        {
            try
            {
                return context.ShoppingCart.Find(model => model.Id == Id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                //TODO : log or manage exceptions
                throw ex;
            }
        }

        public bool UpdateShoppingCart(ShoppingCart item)
        {
            try
            {
                var actionResult = context.ShoppingCart.ReplaceOne(
                   p => p.Id.Equals(item.Id), item);
                return actionResult.IsAcknowledged && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                //TODO : log and manage the exception
                throw ex;
            }
        }
    }
}
