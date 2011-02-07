using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace Simple.Data.MongoDb
{
    [Export("MongoDb", typeof(Adapter))]
    internal class MongoAdapter : Adapter
    {
        private MongoDatabase _database;

        static MongoAdapter()
        {
            var provider = BsonSerializer.SerializationProvider;
            if (provider == null)
                BsonSerializer.SerializationProvider = new DynamicSerializationProvider();
            else if (!(provider is DynamicSerializationProvider))
                throw new SimpleDataException("A serialization provider has already been registered.");
        }

        public MongoAdapter()
        { }

        internal MongoAdapter(MongoDatabase database)
        {
            _database = database;
        }

        public override IEnumerable<IDictionary<string, object>> Find(string tableName, SimpleExpression criteria)
        {
            return new MongoAdapterFinder(this).Find(GetCollection(tableName), criteria);
        }

        public override IDictionary<string, object> Insert(string tableName, IDictionary<string, object> data)
        {
            return new MongoAdapterInserter(this).Insert(GetCollection(tableName), data);
        }

        public override int Update(string tableName, IDictionary<string, object> data, SimpleExpression criteria)
        {
            throw new NotImplementedException();
        }

        public override int Delete(string tableName, SimpleExpression criteria)
        {
            return new MongoAdapterDeleter(this).Delete(GetCollection(tableName), criteria);
        }

        public override IEnumerable<string> GetKeyFieldNames(string tableName)
        {
            yield return "Id";
        }

        internal MongoDatabase GetDatabase()
        {
            return _database;
        }

        protected override void OnSetup()
        {
            var settingsKeys = ((IDictionary<string, object>)Settings).Keys;
            if (settingsKeys.Contains("ConnectionString"))
                _database = MongoDatabase.Create(Settings.ConnectionString);
            
        }

        private MongoCollection<BsonDocument> GetCollection(string collectionName)
        {
            return this.GetDatabase().GetCollection(collectionName);
        }
    }
}