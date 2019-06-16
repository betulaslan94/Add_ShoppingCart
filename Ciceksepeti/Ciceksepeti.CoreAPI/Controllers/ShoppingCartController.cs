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
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpPost("CreateShoppingCart")]
        public ActionResult CreateShoppingCart([FromBody] ShoppingCart Item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string result = _shoppingCartService.CreateShoppingCart(Item);

            if (!object.Equals(result, ResultCodes.OK))
                return BadRequest(new { error_description = result });

            return Ok();
        }

        [HttpPost("UpdateShoppingCart")]
        public ActionResult UpdateShoppingCart([FromBody] ShoppingCart Item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string result = _shoppingCartService.UpdateShoppingCart(Item);

            if (!object.Equals(result, ResultCodes.OK))
                return BadRequest(new { error_description = result });

            return Ok();
        }
    }
}