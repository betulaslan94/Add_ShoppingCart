using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ciceksepeti.Business.Abstract;
using Ciceksepeti.Business.Common;
using Ciceksepeti.Entities.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Ciceksepeti.CoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("AddProducts")]
        public ActionResult AddProducts(List<Product> productList)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string result = _productService.AddProduct(productList);

            if (!object.Equals(result, ResultCodes.OK))
                return BadRequest(new { error_description = result });

            return Ok();
        }
    }
}