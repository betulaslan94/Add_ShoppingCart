using Ciceksepeti.Business.Abstract;
using Ciceksepeti.Business.Common;
using Ciceksepeti.Data.Abstract;
using Ciceksepeti.Data.Repositories;
using Ciceksepeti.Entities;
using Ciceksepeti.Entities.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ciceksepeti.Business.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IProductRepository _productRepository;

        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _productRepository = productRepository;
        }

        /// <summary>
        /// Yeni yaratılacak sepet için ürünün varlığını ve stoğunu kontrol ettikten sonra ekleme işlemi yapar
        /// </summary>
        /// <param name="Item">sepet içeriği</param>
        /// <returns>response code</returns>
        public string CreateShoppingCart(ShoppingCart Item)
        {
            string result = ResultCodes.OK;
            decimal totalPrice = 0;
            try
            {
                var existShoppingCart = _shoppingCartRepository.GetShoppingCartById(Item.Id);

                //gerçekten sepet daha önce yok mu?
                if (object.Equals(existShoppingCart, null))
                {
                    if (!ValidateShoppingCart(Item, ref result))
                        return result;

                    foreach (KeyValuePair<string, int> product in Item.Product)
                    {
                        var productInfo = _productRepository.GetProductById(ObjectId.Parse(product.Key));
                        productInfo.Stock -= product.Value;
                        if (object.Equals(productInfo.Stock, 0))
                            productInfo.IsActive = false;
                        _productRepository.UpdateProduct(productInfo);
                        totalPrice += (productInfo.Price * product.Value);
                    }

                    Item.TotalPrice = totalPrice;
                    _shoppingCartRepository.AddShoppingCart(Item);
                }
                else
                    UpdateShoppingCart(Item);
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }

            return result;
        }

        /// <summary>
        /// Daha önce oluşturulmuş bir sepetin içindeki ürünleri kontrol eder, ürünün varlığını ve stoğunu kontrol ettikten sonra ürün daha önce sepette varsa quantity i artırır, yoksa direk ekler
        /// </summary>
        /// <param name="Item">sepet içeriği</param>
        /// <returns>response code</returns>
        public string UpdateShoppingCart(ShoppingCart Item)
        {
            string result = ResultCodes.OK;

            try
            {
                var existShoppingCart = _shoppingCartRepository.GetShoppingCartById(Item.Id);

                //yeni sepet mi yoksa eski sepetten mi devam ona bakıyor
                if (object.Equals(existShoppingCart, null))
                {
                    CreateShoppingCart(Item);
                }
                else
                {
                    if (!ValidateShoppingCart(Item, ref result))
                        return result;

                    foreach (KeyValuePair<string, int> product in Item.Product)
                    {
                        var productInfo = _productRepository.GetProductById(ObjectId.Parse(product.Key));
                        productInfo.Stock -= product.Value;
                        if (object.Equals(productInfo.Stock, 0))
                            productInfo.IsActive = false;
                        _productRepository.UpdateProduct(productInfo);
                        existShoppingCart.TotalPrice += (productInfo.Price * product.Value);
                    }

                    var keys = new List<string>(Item.Product.Keys);
                    foreach (string key in keys)
                    {
                        bool isSameProduct = existShoppingCart.Product.ContainsKey(key);
                        if (isSameProduct)
                            Item.Product[key] += Convert.ToInt32(existShoppingCart.Product[key]);
                    }

                    _shoppingCartRepository.UpdateShoppingCart(Item);
                }
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }

        #region Validation

        private bool ValidateShoppingCart(ShoppingCart item, ref string result)
        {
            bool retVal = true;
            if (!CheckNull(item, ref result))
                retVal = false;
            else if (!CheckStock(item, ref result))
                retVal = false;

            return retVal;
        }

        private bool CheckNull(ShoppingCart item, ref string result)
        {
            bool retVal = true;
            if (object.Equals(item, null) || object.Equals(item.Product, null) || object.Equals(item.Id, 0) || object.Equals(item.Product.Count, 0) || item.Product.ContainsValue(0))
                result = ResultCodes.MissingParameters;

            if (!object.Equals(result, ResultCodes.OK))
                retVal = false;

            return retVal;
        }

        private bool CheckStock(ShoppingCart item, ref string result)
        {
            foreach (KeyValuePair<string, int> product in item.Product)
            {
                var productInStock = _productRepository.GetProductById(ObjectId.Parse(product.Key));

                if (object.Equals(productInStock, null))
                {
                    result = ResultCodes.NoSuchProduct;
                    return false;
                }
                else if (productInStock.Stock < product.Value)
                {
                    result = ResultCodes.NotInStock;
                    return false;
                }
            }
            return true;
        }

        #endregion
    }
}
