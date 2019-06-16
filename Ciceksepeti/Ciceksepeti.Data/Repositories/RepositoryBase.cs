using Ciceksepeti.Data.Abstract;
using Ciceksepeti.Data.DbContext;
using Ciceksepeti.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ciceksepeti.Data.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T>
        where T : class, IEntityBase
    {
        protected DbContext<T> context = null;

        public RepositoryBase() : base()
        {
            context = context ?? new DbContext<T>();
        }

        public void Add(T entity)
        {
            context.dbModel.InsertOne(entity);
        }

        public bool Delete(ObjectId Id)
        {
            DeleteResult actionResult = context.dbModel.DeleteOne(Builders<T>.Filter.Eq("Id", Id));
            return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
        }

        public bool DeleteAll()
        {
            DeleteResult actionResult
                    =  context.Product.DeleteMany(new BsonDocument());

            return actionResult.IsAcknowledged
                && actionResult.DeletedCount > 0;
        }

        public T GetSingle(ObjectId Id)
        {
            return  context.dbModel.Find(model => model.Id == Id).FirstOrDefault();
        }

        public T GetSingle(Expression<Func<T, bool>> predicate)
        {
            return  context.dbModel.Find(predicate).FirstOrDefault();
        }

        public ObjectId GetInternalId(string Id)
        {
            ObjectId internalId;
            if (!ObjectId.TryParse(Id, out internalId))
                internalId = ObjectId.Empty;

            return internalId;
        }

        public bool Update(T entity)
        {
            ReplaceOneResult actionResult =  context.dbModel.ReplaceOne(
                    p => p.Id.Equals(entity.Id), entity);

            return actionResult.IsAcknowledged && actionResult.ModifiedCount > 0;
        }

    }
}
