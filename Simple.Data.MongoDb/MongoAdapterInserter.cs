using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simple.Data.MongoDb
{
    internal class MongoAdapterInserter
    {
        private readonly MongoAdapter _adapter;

        public MongoAdapterInserter(MongoAdapter adapter)
        {
            if (adapter == null) throw new ArgumentNullException("adapter");
            _adapter = adapter;
        }

        public IDictionary<string, object> Insert(string tableName, IDictionary<string, object> data)
        {
            return null;
        }
    }
}
