using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ciceksepeti.Entities
{
    public interface IEntityBase
    {
        ObjectId Id { get; set; }
    }
}
