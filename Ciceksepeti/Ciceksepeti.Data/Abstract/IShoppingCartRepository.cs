using Ciceksepeti.Entities.Entities;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ciceksepeti.Data.Abstract
{
    public interface IShoppingCartRepository: IRepositoryBase<ShoppingCart>
    {
        ShoppingCart GetShoppingCartById(ObjectId Id);
        void AddShoppingCart(ShoppingCart Item);
        bool UpdateShoppingCart(ShoppingCart Item);
    }
}
