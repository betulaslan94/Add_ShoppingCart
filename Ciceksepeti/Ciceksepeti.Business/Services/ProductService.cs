using Ciceksepeti.Business.Abstract;
using Ciceksepeti.Business.Common;
using Ciceksepeti.Data.Abstract;
using Ciceksepeti.Data.Repositories;
using Ciceksepeti.Entities;
using Ciceksepeti.Entities.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ciceksepeti.Business.Services
{
  public  class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// gelen ürün listesini kontrol eder, ürün varsa daha önce var uyarısı verir yoksa ekler
        /// </summary>
        /// <param name="list">ürün listesi</param>
        /// <returns>response message</returns>
        public string AddProduct(List<Product> list)
        {
            string result = ResultCodes.OK;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(result);

            if (object.Equals(list, null) && !object.Equals(list.Count, 0))
                return ResultCodes.MissingParameters;

            try
            {
                foreach (var item in list)
                {
                    var product = _productRepository.GetProductById(item.Id);

                    //exist product control
                    if (object.Equals(product, null))
                    {
                        _productRepository.AddProduct(item);
                        sb.AppendLine(item.Name + " eklendi");
                    }
                    else
                    {
                        sb.AppendLine(ResultCodes.AlreadyExistProduct + " : " + item.Name);
                    }
                }
            }
            catch(Exception ex)
            {
                result = ex.ToString();
            }
            return sb.ToString();
        }

    }
}
