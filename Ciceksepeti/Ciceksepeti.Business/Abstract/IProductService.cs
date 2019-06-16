using Ciceksepeti.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ciceksepeti.Business.Abstract
{
    public interface IProductService
    {
        string AddProduct(List<Product> Item);
    }
}
