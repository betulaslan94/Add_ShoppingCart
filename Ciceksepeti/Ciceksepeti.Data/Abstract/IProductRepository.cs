using Ciceksepeti.Entities.Entities;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ciceksepeti.Data.Abstract
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        Product GetProductById(ObjectId Id);
        void AddProduct(Product Item);
        bool UpdateProduct(Product Item);
    }
}
