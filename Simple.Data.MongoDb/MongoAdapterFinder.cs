using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Simple.Data.MongoDb
{
    internal class MongoAdapterFinder
    {
        private readonly MongoAdapter _adapter;
        private readonly IExpressionFormatter _expressionFormatter;

        public MongoAdapterFinder(MongoAdapter adapter)
        {
            if (adapter == null) throw new ArgumentNullException("adapter");
            _adapter = adapter;
            _expressionFormatter = new ExpressionFormatter();
        }

        public IEnumerable<IDictionary<string, object>> Find(string collectionName, SimpleExpression criteria)
        {
            if (criteria == null) return FindAll(collectionName);

            var query = _expressionFormatter.Format(criteria);

            var collection = GetCollection(collectionName);
            return collection.Find(query).Select(x => x.ToDictionary());
        }

        public IEnumerable<IDictionary<string, object>> FindAll(string collectionName)
        {
            var collection = GetCollection(collectionName);
            return collection.FindAll().Select(d => d.ToDictionary());
        }

        private MongoCollection<BsonDocument> GetCollection(string collectionName)
        {
            return _adapter.GetDatabase().GetCollection(collectionName);
        }
    }
}