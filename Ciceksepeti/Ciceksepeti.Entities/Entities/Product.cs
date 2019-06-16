using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;
using System.Text;

namespace Ciceksepeti.Entities.Entities
{
   public class Product : IEntityBase
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Name { get; set; }

        public int Stock { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public bool IsActive { get; set; }

        [Range(0.0, Double.MaxValue)]
        public decimal Price { get; set; }

        public Product()
        {
            this.Id = ObjectId.GenerateNewId();
        }
    }
}
