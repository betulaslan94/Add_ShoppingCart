using Microsoft.Extensions.Options;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ciceksepeti.Data.Abstract
{
    public interface IRepositoryBase<T> where T : class
    {
        T GetSingle(ObjectId Id);
        void Add(T entity);
        bool Update(T entity);
        bool Delete(ObjectId Id);
        bool DeleteAll();
    }
}
