using Ciceksepeti.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ciceksepeti.Business.Abstract
{
    public interface IShoppingCartService
    {
        string CreateShoppingCart(ShoppingCart Item);
        string UpdateShoppingCart(ShoppingCart Item);
    }
}
