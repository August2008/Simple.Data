using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Bson;

namespace Simple.Data.MongoDb
{
    public static class BsonDocumentExtensions
    {
        public static IDictionary<string, object> ToDictionary(this BsonDocument document)
        {
            return document.Elements.ToDictionary(x => x.Name, x => ConvertValue(x.Value));
        }

        private static object ConvertValue(BsonValue value)
        {
            if (value.IsBsonDocument)
                return value.AsBsonDocument.ToDictionary();

            return value.RawValue;
        }
    }
}