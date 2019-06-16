using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ciceksepeti.Entities.Entities
{
   public class ShoppingCart : IEntityBase
    {
        [BsonId]
        public ObjectId  Id { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public bool IsActive { get; set; }

        //ilk değer product id, ikinci değer quantity bilgisini tutacak
        public Dictionary<string, int> Product { get; set; }

        public decimal TotalPrice { get; set; }

        public ShoppingCart()
        {
            this.Id = ObjectId.GenerateNewId();
        }
    }
}
