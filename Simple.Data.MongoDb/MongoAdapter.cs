using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

using MongoDB.Driver;

namespace Simple.Data.MongoDb
{
    [Export("MongoDb", typeof(Adapter))]
    internal class MongoAdapter : Adapter, IAdapterWithRelation
    {
        private MongoDatabase _database;

        public MongoAdapter()
        {

        }

        internal MongoAdapter(MongoDatabase database)
        {
            _database = database;
        }

        public override IEnumerable<IDictionary<string, object>> Find(string tableName, SimpleExpression criteria)
        {
            return new MongoAdapterFinder(this).Find(tableName, criteria);
        }

        public override IDictionary<string, object> Insert(string tableName, IDictionary<string, object> data)
        {
            throw new NotImplementedException();
        }

        public override int Update(string tableName, IDictionary<string, object> data, SimpleExpression criteria)
        {
            throw new NotImplementedException();
        }

        public override int Delete(string tableName, SimpleExpression criteria)
        {
            throw new NotImplementedException();
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

        public bool IsValidRelation(string tableName, string relatedTableName)
        {
            return true;
        }

        public IEnumerable<IDictionary<string, object>> FindRelated(string tableName, IDictionary<string, object> row, string relatedTableName)
        {
            return null;
        }
    }
}